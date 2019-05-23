﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS_Cam;
using Pathfinding.Examples;

public class PlayerManager : MonoBehaviour
{

    #region Units in Game Var

    GameObject[] UnitsInGame;
    UnitCara[] UnitsInGameCara;
    UnitCara OnActiveUnit;

    Image[] UnitsInGameDisplay;

    #endregion

    TurnBasedManager m_turnBasedManager;
    [Header("Spells Button Array")]
    public Button[] SpellsButton;

    [Header("ActionPoints Layout")]
    public GameObject ActionPointsLayout;
    [Header("UnitsInGame Layout")]
    public GameObject UnitsInGameLayout;
    [Space]
    public Text m_actionPointsCosts;

    Image[] m_actionPointDisplay;

    RTS_Camera cam;

    int turnCount;
    int turnCountMax;

    int m_onUsedSpell;



    # region Get Set
    public UnitCara _onActiveUnit
    {
        get
        {
            return OnActiveUnit;
        }

        set
        {
            OnActiveUnit = value;
        }
    }

    public TurnBasedManager TurnBasedManager
    {
        get
        {
            return m_turnBasedManager;
        }

        set
        {
            m_turnBasedManager = value;
        }
    }

    public int OnUsedSpell
    {
        get
        {
            return m_onUsedSpell;
        }

        set
        {
            m_onUsedSpell = value;
        }
    }

    public int TurnCount
    {
        get
        {
            return turnCount;
        }

        set
        {
            turnCount = value;
        }
    }

    public UnitCara[] m_UnitsInGameCara
    {
        get
        {
            return UnitsInGameCara;
        }

        set
        {
            UnitsInGameCara = value;
        }
    }

    public Image[] m_UnitsInGameDisplay
    {
        get
        {
            return UnitsInGameDisplay;
        }

        set
        {
            UnitsInGameDisplay = value;
        }
    }
    #endregion 


    private void Awake()
    {
        TurnBasedManager = FindObjectOfType<TurnBasedManager>();
        m_actionPointDisplay = new Image[6];
        m_actionPointDisplay = ActionPointsLayout.GetComponentsInChildren<Image>();
        m_actionPointsCosts.gameObject.SetActive(false);
        m_UnitsInGameDisplay = UnitsInGameLayout.GetComponentsInChildren<Image>();
    }

    public void Start()
    {
        cam = Camera.main.GetComponent<RTS_Camera>();
        ResetArray();
        _onActiveUnit = GetMax().GetComponent<UnitCara>();
        OnTurnPassed();
        InitiateWheelDisplay();


        TurnCount = turnCountMax;
    }

    public void InitiateWheelDisplay()
    {
        for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
        {
            if (i == 0 || i == m_UnitsInGameCara.Length - 1)
            {
                if (i == 0)
                {
                    m_UnitsInGameCara[0].UnitWheelArt = m_UnitsInGameCara[0].unitStats.characterIsFirstArtwork;
                }
                else
                {
                    m_UnitsInGameCara[i].UnitWheelArt = m_UnitsInGameCara[i].unitStats.characterIsLastArtwork;
                }
            }
            else
            {
                m_UnitsInGameCara[i].UnitWheelArt = m_UnitsInGameCara[i].unitStats.characterIsNeitherArtwork;
            }

            m_UnitsInGameDisplay[i].sprite = m_UnitsInGameCara[i].UnitWheelArt;
            m_UnitsInGameCara[i].Courage -= (i/ 10f);
        }
    }

    void ResetArray()
    {
        UnitsInGame = new GameObject[0];
        UnitsInGame = GameObject.FindGameObjectsWithTag("Units");
        m_UnitsInGameCara = new UnitCara[UnitsInGame.Length];

        for (int i = 0, l = UnitsInGame.Length; i < l; ++i)
        {
            if (UnitsInGame[i].GetComponent<UnitCara>() != null)
            {
                m_UnitsInGameCara[i] = UnitsInGame[i].GetComponent<UnitCara>();
                //m_UnitsInGameCara[i].Courage -= i/10;// m_UnitsInGameCara[i].Courage/(i+1*10);

            }
        }
        turnCountMax = m_UnitsInGameCara.Length;
    }

    void OnResetUnitsWheel()
    {
        m_UnitsInGameDisplay = UnitsInGameLayout.GetComponentsInChildren<Image>();
        for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
        {
            m_UnitsInGameDisplay[i].sprite = m_UnitsInGameCara[i].UnitWheelArt;
        }
    }

    public void OnTurnPassed()
    {
        if (!TurnBasedManager.IsMoving && !_onActiveUnit.m_isInAnimation)
        {
            ResetArray();
            TurnCount--;
            OnTableTurnOver();
            //Debug.Log(TurnCount);
            _onActiveUnit = GetMax().GetComponent<UnitCara>();
            for (int i = 0, l = m_UnitsInGameCara.Length ; i < l; ++i)
            {
                m_UnitsInGameCara[i].ActivateSelectedGameObject(false);
                m_UnitsInGameCara[i].WitchNbrInTheList(i);
            }
            _onActiveUnit.ActivateSelectedGameObject(true);
            OnPositionCamera();
            OnChangingUI();
            OnResetUnitsWheel();
            TurnBasedManager.ChangeState(0);
            TurnBasedManager.OnSetMouvement();
            OnPassiveEffectTrigger();
            _onActiveUnit.ArmorBar.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, _onActiveUnit.unitStats.m_armor, _onActiveUnit.ArmorPoint);


            for (int i = 0, l = m_actionPointDisplay.Length; i < l; ++i)
            {
                if (!m_actionPointDisplay[i].gameObject.activeSelf)
                {
                    m_actionPointDisplay[i].gameObject.SetActive(true);
                }
            }

            if (_onActiveUnit._isTaunt)
            {

                TurnBasedManager.StartCoroutine(TurnBasedManager.MoveTowardTarget(_onActiveUnit.GetComponent<TurnBasedAI>(), _onActiveUnit.SpellCaster));

            }
            _onActiveUnit.Courage = _onActiveUnit.Courage / 10;
        }
    }

    public void ActionPointsDisplay(int AP_Left)
    {
        int actionPointMax = 6;
        int actionPointLost = actionPointMax - AP_Left;

        for (int i = 0, l = actionPointLost; i < l; ++i)
        {
            if (m_actionPointDisplay[i].gameObject.activeSelf)
            {
                m_actionPointDisplay[i].gameObject.SetActive(false);
            }
        }
    }

    public void MovementCost(int cost, bool on)
    {
        m_actionPointsCosts.gameObject.SetActive(on);
        if(cost <= 6)
        {
            m_actionPointsCosts.text = string.Format("Costs : {0} PA", cost);
        }
        else
        {
            m_actionPointsCosts.text = string.Format("Not enough PA");
        }
    }

    #region Personnage lance un Spell
    public void OnSpellCast(int SpellNbr)
    {
        OnUsedSpell = SpellNbr;
        if (_onActiveUnit.ActionPoints - _onActiveUnit.Spells1[SpellNbr].cost >= 0 && _onActiveUnit.CoolDownCount[SpellNbr] == 0)
        {
            _onActiveUnit.OnUsingSpell(_onActiveUnit.Spells1[SpellNbr], SpellNbr);
        }
    }
    public void OnButtonHover(int SpellNbr)
    {
        //Debug.Log("HasEnter");
        OnUsedSpell = SpellNbr;
        TurnBasedManager.ChangeState(6); 
    }
    public void OnLeaveHover(int SpellNbr)
    {
        //Debug.Log("HasLeft");
        TurnBasedManager.ChangeState(0);
    }
    #endregion

    public void OnCoolDownDisplay(int SpellNbr)
    {
        if (_onActiveUnit.CoolDownCount[SpellNbr] != 0)
        {
            SpellsButton[SpellNbr].GetComponentInChildren<Text>().text = _onActiveUnit.CoolDownCount[SpellNbr].ToString();
        }
        else
        {
            SpellsButton[SpellNbr].GetComponentInChildren<Text>().text = "Spell " + SpellNbr;
        }
    }

    public void OnPassiveEffectTrigger()
    {
        _onActiveUnit.OnUnitPassiveEffect();
    }

    public void OnCoolDownspell()
    {
        _onActiveUnit.OnCoolDown();
    }

    #region Fin de tour de personnage
    void OnChangingUI()
    {
        for (int i = 0, l = SpellsButton.Length; i < l; ++i)
        {
            SpellsButton[i].image.sprite = _onActiveUnit.Spells1[i].artwork;
            if (_onActiveUnit.CoolDownCount[i] != 0)
            {
                SpellsButton[i].GetComponentInChildren<Text>().text = _onActiveUnit.CoolDownCount[i].ToString();
            }
            else
            {
                SpellsButton[i].GetComponentInChildren<Text>().text = "Spell " + i;
            }
        }
    }

    void OnPositionCamera()
    {
        cam.targetFollow = _onActiveUnit.gameObject.transform;
    }
    #endregion

    #region Fin De Tour De Table
    void OnTableTurnOver()
    {
        if (TurnCount <= 0)
        {
            for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
            {
                if (m_UnitsInGameCara[i].GetComponent<UnitCara>() != null)
                {
                    m_UnitsInGameCara[i].HasPlayed = false;
                    m_UnitsInGameCara[i].Courage = m_UnitsInGameCara[i].Courage * 10;
                    //m_UnitsInGameCara[i].Courage = m_UnitsInGameCara[i].unitStats.m_courage;
                    //m_UnitsInGameCara[i].Courage -= i;
                    m_UnitsInGameCara[i].ReduceCoolDown();
                    if (m_UnitsInGameCara[i].m_IsDebuffed)
                    {
                        m_UnitsInGameCara[i].ReduceDebuff();
                    }
                    m_UnitsInGameCara[i].ReduceTaunt();
                    m_UnitsInGameCara[i].ActionPoints = 6;
                }
            }
            //Debug.Log("Next Turn");
            ResetArray();
            TurnCount = turnCountMax;
        }
    }
    #endregion

    GameObject GetMax()
    {
        
        Array.Sort(m_UnitsInGameCara, delegate (UnitCara x, UnitCara y) { return y.Courage.CompareTo(x.Courage); });

        if (m_UnitsInGameCara != null)
        {
            for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
            {
                if (!m_UnitsInGameCara[i].HasPlayed)
                {
                    GameObject max = m_UnitsInGameCara[i].gameObject;
                    m_UnitsInGameCara[i].HasPlayed = true;
                    //Debug.Log(max.name + " est l'unité avec le plus de courage.");
                    return max;
                }
            }
        }
        //Debug.Log("Atchoum");
        return null;
    }
}
