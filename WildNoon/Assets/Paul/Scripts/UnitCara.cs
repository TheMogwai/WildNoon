using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject Selected;
    int[] coolDownCount;
    PlayerManager player;

    #region Unit Stats
    int _LifePoint;
    int _ArmorPoint;
    int _Courage;
    int _Damage;
    int _Range;
    int _Mobility;


    #endregion


    bool isTeam2;

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

    public int Courage
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
    #endregion
    private void Awake()
    {
        LifePoint = unitStats.m_heatlh;
        ArmorPoint = unitStats.m_armor;
        Courage = unitStats.m_courage;
        Damage = unitStats.m_damage;
        Range = unitStats.m_range;
        Mobility = unitStats.m_mobility;

        ActionPoints = 6;
        ActionPointsPreview = ActionPoints;
        Spells = new Spells[4] { unitStats.firstSpell, unitStats.secondSpell, unitStats.thirdSpell, unitStats.FourthSpell };
        CoolDownCount = new int[4] { 0, 0, 0, 0 };

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
        Player.TurnBasedManager.ChangeState(5);
        OnUsedSpell = Spell;
        OnNbrSpell = i;
    }

    public virtual void OnCoolDown()
    {
        if (CoolDownCount[OnNbrSpell] == 0)
        {
            if (ActionPoints > OnUsedSpell.cost)
            {
                ActionPoints -= OnUsedSpell.cost;
                Player.ActionPointsDisplay(ActionPoints);
                CoolDownMethod(OnNbrSpell);
            }
        }
    }

    Spells OnUsedSpell;
    int OnNbrSpell;
    public virtual void OnUnitPassiveEffect()
    {
        
    }


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
}
