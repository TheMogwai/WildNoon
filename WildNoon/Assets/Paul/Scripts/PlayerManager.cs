using System.Collections;
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
    UnitCara OnUnitPreviouslyActive;

    Image[] UnitsInGameDisplay;

    #endregion

    TurnBasedManager m_turnBasedManager;
    [Header("UnitCara Exsisting")]
    public UnitCara[] allUnits;
    /*[Space]
    [Header("Canvas")]
    public GameObject Canvas;*/
    [Space]
    [Header("Spells Button Array")]
    public Button[] SpellsButton;
    DescriptionPanel[] spellImage;
    Text[] spellDescription;
    [Space]
    [Header("Next Turn Button")]
    public Button NextTurn;
    [Header("ActionPoints Layout")]
    public GameObject ActionPointsLayout;
    [Header("UnitsInGame Layout")]
    public GameObject UnitsInGameLayout;
    [Space]
    [Header("Movement Cost Display")]
    public Text m_actionPointsCosts;
    [Space]
    [Header("Timer Display")]
    public GameObject m_TimerParent;
    Text[] m_TimeLeft;
    Image[] m_SliderTimeLeft;
    float[] timeleft;
    float[] minutes;
    float[] seconds;
    [Space]
    [Header("Timer")]
    public float m_time;
    public float ropeTime;
    [Space]
    [Header("Unit State")]
    public Image m_health;
    public Image m_armor;
    [Space]
    [Header("Team State")]
    //public GameObject m_isTeamParent;
    public Image[] m_teamArtWork;
    public Image[] m_teamHealth;
    public Image[] m_teamArmor;
    UnitCara[] m_team1;
    UnitCara[] m_team2;
    [Space]
    [Header("Player's Turn")]
    public GameObject playerTurnMiddle;
    public GameObject playerTurnTopLeft;
    [Space]
    public Sprite player1;
    public Sprite player2;

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
        /*SpellsButton = new Button[4];
        m_teamArtWork = new Image[4];
        m_teamHealth = new Image[4];
        m_teamArmor = new Image[4];
        for (int i = 0; i < SpellsButton.Length; i++)
        {
            SpellsButton[i] = Canvas.GetComponentInChildren<IsSpellButton>().gameObject.GetComponent<Button>();
        }
        ActionPointsLayout = Canvas.GetComponentInChildren<IsActionPointLayout>().gameObject;
        UnitsInGameLayout = Canvas.GetComponentInChildren<IsUnitInWheelDisplay>().gameObject;
        m_actionPointsCosts = Canvas.GetComponentInChildren<IsCostDisplay>().gameObject.GetComponent<Text>();
        m_TimerParent = Canvas.GetComponentInChildren<IsTimerParent>().gameObject;
        m_isTeamParent = Canvas.GetComponentInChildren<IsTeamParent>().gameObject;

        for (int i = 0; i < 4; i++)
        {
            m_teamArtWork[i] = m_isTeamParent.GetComponentInChildren<IsTeamArtWork>().gameObject.GetComponent<Image>();
            m_teamHealth[i] = m_isTeamParent.GetComponentInChildren<IsTeamHealth>().gameObject.GetComponent<Image>();
            m_teamArmor[i] = m_isTeamParent.GetComponentInChildren<IsTeamArmor>().gameObject.GetComponent<Image>();

        }*/


        TurnBasedManager = FindObjectOfType<TurnBasedManager>();
        m_actionPointDisplay = new Image[6];
        m_actionPointDisplay = ActionPointsLayout.GetComponentsInChildren<Image>();
        m_actionPointsCosts.gameObject.SetActive(false);
        m_UnitsInGameDisplay = UnitsInGameLayout.GetComponentsInChildren<Image>();


        #region Spell Description Var
        spellImage = new DescriptionPanel[SpellsButton.Length];
        spellDescription = new Text[spellImage.Length];

        for (int i = 0, l = SpellsButton.Length; i < l; ++i)
        {
            spellImage[i] = SpellsButton[i].GetComponentInChildren<DescriptionPanel>();
        }
        for (int i = 0, l= spellImage.Length; i < l; ++i)
        {
            spellDescription[i] = spellImage[i].GetComponentInChildren<Text>();
        }
        for (int i = 0, l = SpellsButton.Length; i < l; ++i)
        {
            if (spellImage[i].gameObject.activeSelf)
            {
                spellImage[i].gameObject.SetActive(false);
            }
        }
        #endregion

        #region Time Management Var
        m_TimeLeft = m_TimerParent.GetComponentsInChildren<Text>();
        m_SliderTimeLeft = m_TimerParent.GetComponentsInChildren<Image>();
        timeleft = new float[2];
        minutes = new float[2];
        seconds = new float[2];
        isAtRopeTime = new bool[2] { false, false };
        for (int i = 0; i < timeleft.Length; i++)
        {
            timeleft[i] = m_time;
            minutes[i] = Mathf.Floor(timeleft[i] / 60f);
            seconds[i] = timeleft[i] % 60;
            if (seconds[i] > 59)
            {
                seconds[i] = 59;
            }
            OnSwitchDisplay(i,i);
        }
        #endregion
    }

    public void Start()
    {
        cam = Camera.main.GetComponent<RTS_Camera>();
        ResetArray();
        _onActiveUnit = GetMax().GetComponent<UnitCara>();
        OnTurnPassed();
        InitiateWheelDisplay();
        for (int i = 0; i < UnitsInGameCara.Length; i++)
        {
            if (UnitsInGameCara[i].m_canvas.gameObject.activeSelf)
            {
                UnitsInGameCara[i].m_canvas.gameObject.SetActive(false);
            }
        }

        TurnCount = turnCountMax;

        TeamDisplay();

    }

    private void Update()
    {
        RopeMethod();
        TimerMethod();
        if (m_actionPointsCosts.gameObject.activeSelf)
        {

            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x < (Screen.width- (m_actionPointsCosts.gameObject.GetComponent<RectTransform>().rect.width/2)* m_actionPointsCosts.gameObject.transform.localScale.x))
            {
                m_actionPointsCosts.gameObject.transform.position = new Vector3(mousePosition.x+((m_actionPointsCosts.gameObject.GetComponent<RectTransform>().rect.width * m_actionPointsCosts.gameObject.transform.localScale.x )/ 3), mousePosition.y, mousePosition.z);
            }
            else
            {
                m_actionPointsCosts.gameObject.transform.position = new Vector3(mousePosition.x - ((m_actionPointsCosts.gameObject.GetComponent<RectTransform>().rect.width * m_actionPointsCosts.gameObject.transform.localScale.x) / 3), mousePosition.y, mousePosition.z);
            }
        }
    }

    void TeamDisplay()
    {
        m_team1 = FindObjectOfType<InGameSpawner>().m_Team_1_Root.GetComponentsInChildren<UnitCara>();        
        m_team2 = FindObjectOfType<InGameSpawner>().m_Team_2_Root.GetComponentsInChildren<UnitCara>();
        if (!_onActiveUnit.IsTeam2)
        {
            for (int i = 0, l = m_team1.Length; i < l; ++i)
            {
                m_teamArtWork[i].sprite = m_team1[i].unitStats.characterArtwork;
                m_teamHealth[i].fillAmount = Mathf.InverseLerp(0, m_team1[i].unitStats.m_heatlh, m_team1[i].LifePoint);
                m_teamArmor[i].fillAmount = Mathf.InverseLerp(0, m_team1[i].unitStats.m_armor, m_team1[i].ArmorPoint);
            }
            playerTurnTopLeft.GetComponent<Image>().sprite = player1;
        }
        else
        {
            for (int i = 0, l = m_team2.Length; i < l; ++i)
            {
                m_teamArtWork[i].sprite = m_team2[i].unitStats.characterArtwork;
                m_teamHealth[i].fillAmount = Mathf.InverseLerp(0, m_team2[i].unitStats.m_heatlh, m_team2[i].LifePoint);
                m_teamArmor[i].fillAmount = Mathf.InverseLerp(0, m_team2[i].unitStats.m_armor, m_team2[i].ArmorPoint);
            }
            playerTurnTopLeft.GetComponent<Image>().sprite = player2;
        }
        StartCoroutine(ShowWhoseTurn(_onActiveUnit.IsTeam2));
    }

    IEnumerator ShowWhoseTurn(bool IsTeam2)
    {
        if (!IsTeam2)
        {
            playerTurnMiddle.SetActive(true);
            playerTurnMiddle.GetComponent<Image>().sprite = player1;
            yield return new WaitForSeconds(1f);
            playerTurnMiddle.SetActive(false);
        }
        else
        {
            playerTurnMiddle.SetActive(true);
            playerTurnMiddle.GetComponent<Image>().sprite = player2;
            yield return new WaitForSeconds(1f);
            playerTurnMiddle.SetActive(false);
        }
        StopCoroutine(ShowWhoseTurn(false));
    }

    #region Time Manager
    void TimerMethod()
    {
        if(OnUnitPreviouslyActive.IsTeam2 != _onActiveUnit.IsTeam2)
        {
            if (!_onActiveUnit.IsTeam2)
            {
                
                TimeCount(0);
                OnSwitchDisplay(0, 0);
                OnSwitchDisplay(1, 1);
                
            }
            else
            {
                
                TimeCount(1);

                OnSwitchDisplay(0, 1);
                OnSwitchDisplay(1, 0);
                
            }
        }
        else
        {
            if (!_onActiveUnit.IsTeam2)
            {
                
                TimeCount(0);

                OnSwitchDisplay(0, 0);
            }
            else
            {
                
                TimeCount(1);

                OnSwitchDisplay(0, 1);
               
            }
        }
    }

    void OnSwitchDisplay(int i, int a)
    {
        if(m_TimeLeft[i] != null)
        {
            if (seconds[a] >= 10)
            {
                m_TimeLeft[i].text = string.Format(" {0}:{1}", minutes[a], (int)seconds[a]);
            }
            else
            {
                m_TimeLeft[i].text = string.Format(" {0}:0{1}", minutes[a], (int)seconds[a]);
            }
        }

        if(m_SliderTimeLeft[i] != null)
        {
            m_SliderTimeLeft[i].fillAmount = Mathf.InverseLerp(0, m_time, timeleft[a]);
        }
    }

    void RopeMethod()
    {
        for (int i = 0, l = timeleft.Length; i < l; ++i)
        {
            if(timeleft[i] <= 0)
            {
                timeleft[i] = ropeTime;
                OnTurnPassed();
            }
        }
    }
    bool[] isAtRopeTime; 
    void TimeCount(int i)
    {
        timeleft[i] -= Time.deltaTime;
        minutes[i] = Mathf.Floor(timeleft[i] / 60f);
        seconds[i] = timeleft[i] % 60;
        if (seconds[i] > 59)
        {
            seconds[i] = 59;
        }

        if(minutes[i] <= 0 && seconds[i] <= 0)
        {
            minutes[i] = 0;
            if (!isAtRopeTime[i])
            {
                seconds[i] = ropeTime;
                isAtRopeTime[i] = true;
            }
        }
    }

    #endregion

    #region Wheel Methods

    public void InitiateWheelDisplay()
    {
        for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
        {
            if (i == 0 || i == m_UnitsInGameCara.Length - 1)
            {
                if (!m_UnitsInGameCara[i].IsTeam2)
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
                    if (i == 0)
                    {
                        m_UnitsInGameCara[0].UnitWheelArt = m_UnitsInGameCara[0].unitStats.characterIsFirstArtwork2;
                    }
                    else
                    {
                        m_UnitsInGameCara[i].UnitWheelArt = m_UnitsInGameCara[i].unitStats.characterIsLastArtwork2;
                    }
                }
            }
            else
            {
                if (!m_UnitsInGameCara[i].IsTeam2)
                {
                    m_UnitsInGameCara[i].UnitWheelArt = m_UnitsInGameCara[i].unitStats.characterIsNeitherArtwork;
                }
                else
                {
                    m_UnitsInGameCara[i].UnitWheelArt = m_UnitsInGameCara[i].unitStats.characterIsNeitherArtwork2;
                }
            }

            m_UnitsInGameDisplay[i].sprite = m_UnitsInGameCara[i].UnitWheelArt;
            m_UnitsInGameCara[i].Courage -= (i/ 10f);
        }
    }

    void OnResetUnitsWheel()
    {
        m_UnitsInGameDisplay = UnitsInGameLayout.GetComponentsInChildren<Image>();
        for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
        {
            m_UnitsInGameDisplay[i].sprite = m_UnitsInGameCara[i].UnitWheelArt;
        }
    }

    #endregion

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
            }
        }
        turnCountMax = m_UnitsInGameCara.Length;
    }

    public void OnTurnPassed()
    {
        if (!TurnBasedManager.IsMoving && !_onActiveUnit.m_isInAnimation)
        {
            ResetArray();
            TurnCount--;
            OnTableTurnOver();
            if (_onActiveUnit !=null)
            {
                OnUnitPreviouslyActive = _onActiveUnit;
            }
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
            TeamDisplay();
            for (int i = 0; i < 2; i++)
            {
                if (isAtRopeTime[i])
                {
                    seconds[i] = ropeTime;
                }
            }
            for (int i = 0, l = m_actionPointDisplay.Length; i < l; ++i)
            {
                if (!m_actionPointDisplay[i].gameObject.activeSelf)
                {
                    m_actionPointDisplay[i].gameObject.SetActive(true);
                }
            }

            OnEffectCheck();
            _onActiveUnit.Courage = _onActiveUnit.Courage / 10;
        }
    }

    void OnEffectCheck()
    {
        if (_onActiveUnit._isTaunt)
        {
            if (_onActiveUnit.SpellCaster != null)
            {
                TurnBasedManager.StartCoroutine(TurnBasedManager.MoveTowardTarget(_onActiveUnit.GetComponent<TurnBasedAI>(), _onActiveUnit.SpellCaster));
                for (int i = 0, l = SpellsButton.Length; i < l; ++i)
                {
                    SpellsButton[i].interactable = false;
                }
                NextTurn.interactable = false;
            }
            else
            {
                _onActiveUnit.TimerTaunt = 0;
                _onActiveUnit._isTaunt = false;
            }
        }else if (_onActiveUnit.IsStun1)
        {
            for (int i = 0, l = SpellsButton.Length; i < l; ++i)
            {
                SpellsButton[i].interactable = false;
            }
            NextTurn.interactable = true;
        }
        else
        {
            for (int i = 0, l = SpellsButton.Length; i < l; ++i)
            {
                SpellsButton[i].interactable = true;
            }
            NextTurn.interactable = true;
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
            m_actionPointsCosts.text = string.Format("{0} PA", cost);
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
        if(_onActiveUnit.unitStats == allUnits[0].unitStats && SpellNbr == 3)
        {
            OnUsedSpell = SpellNbr;
            TurnBasedManager.ChangeState(6); 
        }else if (_onActiveUnit.unitStats == allUnits[1].unitStats && SpellNbr == 3)
        {
            OnUsedSpell = SpellNbr;
            TurnBasedManager.ChangeState(10);
        }
        spellImage[SpellNbr].gameObject.SetActive(true);
        spellDescription[SpellNbr].text = _onActiveUnit.Spells1[SpellNbr].spell_Description;
    }
    public void OnLeaveHover(int SpellNbr)
    {
        if ((_onActiveUnit.unitStats == allUnits[0].unitStats || _onActiveUnit.unitStats == allUnits[1].unitStats) && SpellNbr == 3)
        {
            TurnBasedManager.ChangeState(0);
        }
        spellImage[SpellNbr].gameObject.SetActive(false);

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

        m_armor.fillAmount = Mathf.InverseLerp(0, _onActiveUnit.unitStats.m_armor, _onActiveUnit.ArmorPoint);
        m_health.fillAmount = Mathf.InverseLerp(0, _onActiveUnit.unitStats.m_heatlh, _onActiveUnit.LifePoint);
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
                    m_UnitsInGameCara[i].ReduceStun();
                    m_UnitsInGameCara[i].ActionPoints = 6;
                }
            }
            //Debug.Log("Next Turn");
            ResetArray();
            TurnCount = turnCountMax;
        }
        for (int i = 0, l = m_UnitsInGameCara.Length; i < l; ++i)
        {
            if (m_UnitsInGameCara[i].GetComponent<UnitCara>() != null)
            {
                m_UnitsInGameCara[i].IsStunByLasso = false;
            }
        }
        if (_onActiveUnit.HasUsedLasso)
        {
            _onActiveUnit.CoolDownMethod(1);
            OnCoolDownDisplay(1);
            _onActiveUnit.HasUsedLasso = false;
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
                    return max;
                }
            }
        }
        return null;
    }
}
