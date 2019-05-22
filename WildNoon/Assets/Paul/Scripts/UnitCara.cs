using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using Pathfinding.Examples;

public class UnitCara : MonoBehaviour {

    /*[Header("Unit Stats")]
    public int m_courage;
    public int m_heatlh;
    public int m_armor;
    public int m_damage;
    public int m_range;
    public int m_mobility;
    int m_actionPoints;
    bool hasPlayed = false;
    [Space]
    [Header("Unit Spells")]
    public Spells firstSpell;
    public Spells secondSpell;
    public Spells thirdSpell;
    public Spells FourthSpell;
    Spells[] Spells;*/
    int m_actionPoints;
    int m_actionPointsPreview;
    bool hasPlayed = false;
    Spells[] Spells;
    public Characters unitStats;
    public GameObject ArmorBar;
    public GameObject LifeBar;
    public GameObject m_canvas;


    public GameObject Selected;

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

        //m_vfx = FxParents.GetComponentsInChildren<GameObject>();
        for (int i = 0, l = m_vfx.Length; i < l; ++i)
        {
            if (m_vfx[i].activeSelf)
            {
                m_vfx[i].SetActive(false);
            }
        }

        Player = FindObjectOfType<PlayerManager>();
    }

    public void Start()
    {
        if (GetComponentInParent<Team_Check>() != null)
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            isTeam2 = true;
        }
        else
        {
            isTeam2 = false;
        }
    }
    private void Update()
    {
        m_canvas.transform.LookAt(Camera.main.transform);
        //ArmorBar.transform.LookAt(Camera.main.transform);
        //LifeBar.transform.LookAt(Camera.main.transform);
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

    public void IsTaunt(int nbrOfTurnDebuffed, int i)
    {
        isTaunt = true;
        nbrFxActif = i;
        TimerTaunt = nbrOfTurnDebuffed;
        m_vfx[nbrFxActif].SetActive(isTaunt);
    }


    public void ReduceTaunt()
    {
        if (isTaunt && TimerTaunt - 1 > 0)
        {
            TimerTaunt--;
        }
        else
        {
            isTaunt = false;
            m_vfx[nbrFxActif].SetActive(m_IsDebuffed);
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
        //Debug.Log("armor : "+armorpercent);
        float random = Random.Range(0, 100);
        //Debug.Log("random : " + random);
        if (random < armorpercent)
        {
            //Debug.Log(false);
            return false;

        }
        else
        {
            //Debug.Log(gameObject.name + true);
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
        //Debug.Log("Prout");
        if ((LifePoint + ArmorPoint)-damage > 0)
        {
            if(ArmorPoint - damage >= 0)
            {
                Debug.Log("Attack");
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
