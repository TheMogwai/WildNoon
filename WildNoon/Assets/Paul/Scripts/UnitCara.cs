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
    bool hasPlayed = false;
    Spells[] Spells;
    public Characters unitStats;
    int[] coolDownCount;
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
    #endregion
    private void Awake()
    {
        
    }

    public void Start()
    {
        m_actionPoints = 6;
        Spells = new Spells[4] { unitStats.firstSpell, unitStats.secondSpell, unitStats.thirdSpell, unitStats.FourthSpell };
        CoolDownCount = new int[4] { 0,0,0,0 };
    }


    public virtual void OnUsingSpell(Spells Spell, int i)
    {
        //Debug.Log(Spell + " a encore" + Spell.CoolDownCount + " avant d'etre utilisable.");
        if(CoolDownCount[i] == 0)
        {
            if (m_actionPoints > Spell.cost)
            {
                m_actionPoints -= Spell.cost;
                CoolDownMethod(i);
            }
        }
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
