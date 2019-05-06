using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Spawer : MonoBehaviour
{
    UnitStatsButtons[] m_Team_1 = new UnitStatsButtons[4];
    UnitStatsButtons[] m_Team_2 = new UnitStatsButtons[4];

    public UnitStatsButtons[] Team_1
    {
        get
        {
            return m_Team_1;
        }

        set
        {
            m_Team_1 = value;
        }
    }

    public UnitStatsButtons[] Team_2
    {
        get
        {
            return m_Team_2;
        }

        set
        {
            m_Team_2 = value;
        }
    }
}
