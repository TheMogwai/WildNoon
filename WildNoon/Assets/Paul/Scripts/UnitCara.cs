using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using Pathfinding.Examples;

public class UnitCara : MonoBehaviour {

    int m_actionPoints;
    int m_actionPointsPreview;
    bool hasPlayed = false;
    Spells[] Spells;
    public Characters unitStats;
    [Space]
    [Header("LifeBar Var")]
    public GameObject ArmorBar;
    public GameObject LifeBar;
    public GameObject m_canvas;
    public float m_timeShowingLifeBar;
    float _timeToShowLifeBar;
    bool lifebarOn;
    [Space]
    [Header("Selection Nodes")]

    public GameObject Selected;

    [Space]
    [Header("Fx Var")]

    public GameObject FxParents;

    public GameObject[] m_vfx;

    int[] coolDownCount;
    PlayerManager player;


    #region Unit Stats
    int _LifePoint;
    int _ArmorPoint;
    float _Courage;
    int _Damage;
    int _Range;
    int _Mobility;
    int _AutoAttackCost;

    Sprite _UnitWheelArt;

    #endregion


    bool isTeam2;
    Spells OnUsedSpell;
    int OnNbrSpell;

    bool _jimPassifEffect;
    bool _isStunByLasso;
    bool _hasUsedLasso;

    #region Get Set
    public bool HasPlayed
    {
        get
        {
            return hasPlayed;
        }

        set
        {
            hasPlayed = value;
        }
    }

    public Spells[] Spells1
    {
        get
        {
            return Spells;
        }

        set
        {
            Spells = value;
        }
    }

    public int[] CoolDownCount
    {
        get
        {
            return coolDownCount;
        }

        set
        {
            coolDownCount = value;
        }
    }

    public bool IsTeam2
    {
        get
        {
            return isTeam2;
        }

        set
        {
            isTeam2 = value;
        }
    }

    public int LifePoint
    {
        get
        {
            return _LifePoint;
        }

        set
        {
            _LifePoint = value;
        }
    }

    public int ArmorPoint
    {
        get
        {
            return _ArmorPoint;
        }

        set
        {
            _ArmorPoint = value;
        }
    }

    public float Courage
    {
        get
        {
            return _Courage;
        }

        set
        {
            _Courage = value;
        }
    }

    public int Damage
    {
        get
        {
            return _Damage;
        }

        set
        {
            _Damage = value;
        }
    }

    public int Range
    {
        get
        {
            return _Range;
        }

        set
        {
            _Range = value;
        }
    }

    public int Mobility
    {
        get
        {
            return _Mobility;
        }

        set
        {
            _Mobility = value;
        }
    }

    public int ActionPoints
    {
        get
        {
            return m_actionPoints;
        }

        set
        {
            m_actionPoints = value;
        }
    }

    public int ActionPointsPreview
    {
        get
        {
            return m_actionPointsPreview;
        }

        set
        {
            m_actionPointsPreview = value;
        }
    }

    public PlayerManager Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public Spells OnUsedSpell1
    {
        get
        {
            return OnUsedSpell;
        }

        set
        {
            OnUsedSpell = value;
        }
    }

    public Sprite UnitWheelArt
    {
        get
        {
            return _UnitWheelArt;
        }

        set
        {
            _UnitWheelArt = value;
        }
    }

    public bool m_isInAnimation
    {
        get
        {
            return IsInAnimation;
        }

        set
        {
            IsInAnimation = value;
        }
    }

    public bool m_IsDebuffed
    {
        get
        {
            return isDebuffed;
        }

        set
        {
            isDebuffed = value;
        }
    }

    public int TimeDebuffed
    {
        get
        {
            return timeDebuffed;
        }

        set
        {
            timeDebuffed = value;
        }
    }

    public int AutoAttackCost
    {
        get
        {
            return _AutoAttackCost;
        }

        set
        {
            _AutoAttackCost = value;
        }
    }

    public int TimerTaunt
    {
        get
        {
            return timerTaunt;
        }

        set
        {
            timerTaunt = value;
        }
    }

    public bool _isTaunt
    {
        get
        {
            return isTaunt;
        }

        set
        {
            isTaunt = value;
        }
    }

    public TurnBasedAI SpellCaster
    {
        get
        {
            return spellCaster;
        }

        set
        {
            spellCaster = value;
        }
    }

    public bool LifebarOn
    {
        get
        {
            return lifebarOn;
        }

        set
        {
            lifebarOn = value;
        }
    }

    public float TimeToShowLifeBar
    {
        get
        {
            return _timeToShowLifeBar;
        }

        set
        {
            _timeToShowLifeBar = value;
        }
    }

    public bool JimPassifEffect
    {
        get
        {
            return _jimPassifEffect;
        }

        set
        {
            _jimPassifEffect = value;
        }
    }

    public bool IsStunByLasso
    {
        get
        {
            return _isStunByLasso;
        }

        set
        {
            _isStunByLasso = value;
        }
    }

    public bool HasUsedLasso
    {
        get
        {
            return _hasUsedLasso;
        }

        set
        {
            _hasUsedLasso = value;
        }
    }

    public bool IsStun1
    {
        get
        {
            return _isStun;
        }

        set
        {
            _isStun = value;
        }
    }
    #endregion
    private void Awake()
    {
        LifePoint = unitStats.m_heatlh;
        ArmorPoint = unitStats.m_armor;
        Courage = unitStats.m_courage;
        Damage = unitStats.m_damage;
        Range = unitStats.m_range;
        Mobility = unitStats.m_mobility;
        UnitWheelArt = unitStats.characterArtwork;
        AutoAttackCost = unitStats.m_autoAttackCost;

        ActionPoints = 6;
        ActionPointsPreview = ActionPoints;
        Spells = new Spells[4] { unitStats.firstSpell, unitStats.secondSpell, unitStats.thirdSpell, unitStats.FourthSpell };
        CoolDownCount = new int[4] { 0, 0, 0, 0 };

        
        for (int i = 0, l = m_vfx.Length; i < l; ++i)
        {
            if (m_vfx[i].activeSelf)
            {
                m_vfx[i].SetActive(false);
            }
        }

        Player = FindObjectOfType<PlayerManager>();
        if (GetComponentInParent<Team_Check>() != null)
        {
            //gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            isTeam2 = true;
        }
        else
        {
            isTeam2 = false;
        }
    }

    public void Start()
    {
        
    }
    private void Update()
    {
        m_canvas.transform.LookAt(Camera.main.transform);
        ShowingLifeBar();

        m_vfx[2].SetActive(_jimPassifEffect);                   //Fx Indiquant que Jim fera plus de dégat à cette cible
        m_vfx[0].SetActive(_isStunByLasso);                     //Fx Indiquant que la cible est stun par le lasso
        m_vfx[1].SetActive(_isStun);                            //Fx Indiquant que la cible est stun


    }




    void ShowingLifeBar()
    {
        if (LifebarOn && TimeToShowLifeBar > 0)
        {
            TimeToShowLifeBar -= Time.deltaTime;
            m_canvas.gameObject.SetActive(true);

        }
        else
        {
            LifebarOn = false;
            m_canvas.gameObject.SetActive(false);
            TimeToShowLifeBar = m_timeShowingLifeBar;
        }
    }

    public void ActivateSelectedGameObject(bool b)
    {
        if(Selected != null)
        {
            Selected.SetActive(b);
        }
    }


    public virtual void OnUsingSpell(Spells Spell, int i)
    {
        //Debug.Log(Spell + " a encore" + Spell.CoolDownCount + " avant d'etre utilisable.");
        OnUsedSpell1 = Spell;
        OnNbrSpell = i;
    }

    public virtual void OnCoolDown()
    {
        ActionPoints -= OnUsedSpell1.cost;
        Player.ActionPointsDisplay(ActionPoints);
        CoolDownMethod(OnNbrSpell);
    }

    public virtual void OnUnitPassiveEffect()
    {
        
    }

    public virtual void AutoAttack(UnitCara target)
    {
    }



    #region Debuff Methods

    bool isDebuffed;
    int timeDebuffed;
    int nbrFxActif;

    public void IsDebuffed(int nbrOfTurnDebuffed, int i)
    {
        m_IsDebuffed = true;
        nbrFxActif = i;
        TimeDebuffed = nbrOfTurnDebuffed;
        m_vfx[nbrFxActif].SetActive(m_IsDebuffed);
    }


    public void ReduceDebuff()
    {
        if (m_IsDebuffed && TimeDebuffed - 1 > 0)
        {
            TimeDebuffed--;
        }
        else
        {
            m_IsDebuffed = false;
            m_vfx[nbrFxActif].SetActive(m_IsDebuffed);
            ResetStatsAfterDebuff();
        }
    }

    public void ResetStatsAfterDebuff()
    {
        Courage = unitStats.m_courage;
        Damage = unitStats.m_damage;
        Range = unitStats.m_range;
        Mobility = unitStats.m_mobility;
    }

    /// <summary>
    /// 
    /// </summary>
    bool isTaunt;
    int timerTaunt;
    TurnBasedAI spellCaster;

    public void IsTaunt(int nbrOfTurnDebuffed, int i, TurnBasedAI Caster)
    {
        _isTaunt = true;
        SpellCaster = Caster;
        nbrFxActif = i;
        TimerTaunt = nbrOfTurnDebuffed;
        m_vfx[nbrFxActif].SetActive(_isTaunt);
    }


    public void ReduceTaunt()
    {
        if (_isTaunt && TimerTaunt - 1 > 0)
        {
            TimerTaunt--;
        }
        else
        {
            _isTaunt = false;
            m_vfx[nbrFxActif].SetActive(m_IsDebuffed);
        }
    }

    bool _isStun;
    int timerStun;

    public void IsStun(int nbrOfTurnDebuffed)
    {
        _isStun = true;
        timerStun = nbrOfTurnDebuffed;
    }

    public void ReduceStun()
    {
        if (_isStun && timerStun - 1 > 0)
        {
            timerStun--;
        }
        else
        {
            _isStun = false;
        }
    }

    #endregion

    public void ReduceCoolDown()
    {
        for (int i = 0 , l = Spells.Length; i < l; ++i)
        {
            if(CoolDownCount[i] != 0)
            {
                CoolDownMethod(i);
            }
        }
    }

    public bool OnCheckIfCCWorks()
    {
        float armorpercent = Mathf.InverseLerp(0, unitStats.m_armor, ArmorPoint)*100;
        float random = Random.Range(0, 100);
        if (random < armorpercent)
        {
            return false;

        }
        else
        {
            return true;

        }
    }


    public int CoolDownMethod(int i)
    {
        if (CoolDownCount[i] > 0)
        {
            CoolDownCount[i]--;
        }
        else
        {
            CoolDownCount[i] = Spells[i].coolDown;
        }
        return CoolDownCount[i];
    }


    public void OnTakingDamage(int damage)
    {
        if (!m_canvas.gameObject.activeSelf)
        {
            LifebarOn = true;
            TimeToShowLifeBar = m_timeShowingLifeBar;
        }

        if ((LifePoint + ArmorPoint)-damage > 0)
        {
            if(ArmorPoint - damage >= 0)
            {
                ArmorPoint -= damage;
            }
            else
            {
                ArmorPoint -= damage;
                LifePoint += ArmorPoint;
                ArmorPoint = 0;
            }

            if(LifePoint <= 0)
            {
                LifePoint = 0;
                OnUnitDead();
            }
        }
        else
        {
            LifePoint = 0;
            OnUnitDead();
        }
        ArmorBar.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, unitStats.m_armor, ArmorPoint);
        LifeBar.GetComponent<Image>().fillAmount = Mathf.InverseLerp(0, unitStats.m_heatlh, LifePoint);
    }

    void OnUnitDead()
    {
        player.TurnCount--;
        StartCoroutine(AniamtionDeath());
    }

    int nbrInTheList;
    bool IsInAnimation;
    public void WitchNbrInTheList(int i)
    {
        nbrInTheList = i;
    }

    IEnumerator AniamtionDeath()
    {
        m_isInAnimation = true;
        yield return new WaitForSeconds(1f);             //Temps de l'animation de la mort

        if(Player.m_UnitsInGameDisplay[nbrInTheList].sprite == Player.m_UnitsInGameCara[nbrInTheList].unitStats.characterIsFirstArtwork)
        {
            if(nbrInTheList == Player.m_UnitsInGameDisplay.Length - 1)
            {
                Player.m_UnitsInGameDisplay[0].sprite = Player.m_UnitsInGameCara[0].unitStats.characterIsFirstArtwork;
                Player.m_UnitsInGameCara[0]._UnitWheelArt = Player.m_UnitsInGameDisplay[0].sprite;
            }
            else
            {
                Player.m_UnitsInGameDisplay[nbrInTheList + 1].sprite = Player.m_UnitsInGameCara[nbrInTheList + 1].unitStats.characterIsFirstArtwork;
                Player.m_UnitsInGameCara[nbrInTheList + 1]._UnitWheelArt = Player.m_UnitsInGameDisplay[nbrInTheList + 1].sprite;
            }
        }
        else if (Player.m_UnitsInGameDisplay[nbrInTheList].sprite == Player.m_UnitsInGameCara[nbrInTheList].unitStats.characterIsLastArtwork)
        {
            if(nbrInTheList == 0)
            {
                Player.m_UnitsInGameDisplay[Player.m_UnitsInGameDisplay.Length - 1].sprite = Player.m_UnitsInGameCara[Player.m_UnitsInGameDisplay.Length - 1].unitStats.characterIsFirstArtwork;
                Player.m_UnitsInGameCara[Player.m_UnitsInGameDisplay.Length - 1]._UnitWheelArt = Player.m_UnitsInGameDisplay[Player.m_UnitsInGameDisplay.Length - 1].sprite;
            }
            else
            {
                Player.m_UnitsInGameDisplay[nbrInTheList - 1].sprite = Player.m_UnitsInGameCara[nbrInTheList - 1].unitStats.characterIsLastArtwork;
                Player.m_UnitsInGameCara[nbrInTheList - 1]._UnitWheelArt = Player.m_UnitsInGameDisplay[nbrInTheList - 1].sprite;
            }
        }
        Destroy(Player.m_UnitsInGameDisplay[nbrInTheList].gameObject);
        GetComponent<TurnBasedAI>().blocker.Unblock();
        Destroy(gameObject);
        m_isInAnimation = false;

    }
}
