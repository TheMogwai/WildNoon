﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS_Cam;

public class PlayerManager : MonoBehaviour
{

    #region Units in Game Var

    GameObject[] UnitsInGame;
    UnitCara[] UnitsInGameCara;
    UnitCara OnActiveUnit;

    #endregion


    public Button[] SpellsButton;

    RTS_Camera cam;

    int turnCount;

    

    private void Awake()
    {
        cam = Camera.main.GetComponent<RTS_Camera>();
        UnitsInGame = GameObject.FindGameObjectsWithTag("Units");
    }

    public void Start()
    {
        UnitsInGameCara = new UnitCara[UnitsInGame.Length];

        for (int i = 0, l = UnitsInGame.Length; i < l; ++i)
        {
            if(UnitsInGame[i].GetComponent<UnitCara>() != null)
            {
                UnitsInGameCara[i] = UnitsInGame[i].GetComponent<UnitCara>();
            }
        }
        turnCount = UnitsInGame.Length;
        OnTurnPassed();
    }

    public void OnTurnPassed()
    {
        turnCount--;
        OnActiveUnit = GetMax().GetComponent<UnitCara>();
        OnPositionCamera();
        OnTableTurnOver();
        OnChangingUI();
    }

    #region Personnage lance un Spell
    public void OnSpellCast(int SpellNbr)
    {
        OnActiveUnit.OnUsingSpell(OnActiveUnit.Spells1[SpellNbr], SpellNbr);

        if (OnActiveUnit.CoolDownCount[SpellNbr] != 0)
        {
            SpellsButton[SpellNbr].GetComponentInChildren<Text>().text = OnActiveUnit.CoolDownCount[SpellNbr].ToString();
        }
        else
        {
            SpellsButton[SpellNbr].GetComponentInChildren<Text>().text = "Spell " + SpellNbr;
        }
    }
    #endregion

    #region Fin de tour de personnage
    void OnChangingUI()
    {
        for (int i = 0, l = SpellsButton.Length; i < l; ++i)
        {
            SpellsButton[i].image.sprite = OnActiveUnit.Spells1[i].artwork;
            if (OnActiveUnit.CoolDownCount[i] != 0)
            {
                SpellsButton[i].GetComponentInChildren<Text>().text = OnActiveUnit.CoolDownCount[i].ToString();
            }
            else
            {
                SpellsButton[i].GetComponentInChildren<Text>().text = "Spell " + i;
            }
        }
    }

    void OnPositionCamera()
    {
        cam.targetFollow = OnActiveUnit.gameObject.transform;
    }
    #endregion

    #region Fin De Tour De Table
    void OnTableTurnOver()
    {
        if (turnCount == 0)
        {
            for (int i = 0, l = UnitsInGameCara.Length; i < l; ++i)
            {
                if (UnitsInGameCara[i].GetComponent<UnitCara>() != null)
                {
                    UnitsInGameCara[i].HasPlayed = false;
                    UnitsInGameCara[i].ReduceCoolDown();
                    UnitsInGameCara[i].ActionPoints = 6;
                }
            }
            Debug.Log("Next Turn");
            turnCount = UnitsInGame.Length;
        }
    }
    #endregion

    GameObject GetMax()
    {
        Array.Sort(UnitsInGameCara, delegate (UnitCara x, UnitCara y) { return y.unitStats.m_courage.CompareTo(x.unitStats.m_courage); });

        if(UnitsInGameCara != null)
        {
            for (int i = 0, l = UnitsInGameCara.Length; i < l; ++i)
            {
                if (!UnitsInGameCara[i].HasPlayed)
                {
                    GameObject max = UnitsInGameCara[i].gameObject;
                    UnitsInGameCara[i].HasPlayed = true;
                    //Debug.Log(max.name + " est l'unité avec le plus de courage.");
                    return max;
                }
            }
        }
        return null;
    }
}
