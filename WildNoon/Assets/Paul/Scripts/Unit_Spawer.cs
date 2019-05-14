using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Unit_Spawer : MonoBehaviour
{
    GameObject[] team1 = new GameObject[4];
    GameObject[] team2 = new GameObject[4];

    public int[] Team_1_AsInt = new int[4];
    public int[] Team_2_AsInt = new int[4];

    public GameObject[] unitsPrefab;

    #region Get Set
    public GameObject[] Team1
    {
        get
        {
            return team1;
        }

        set
        {
            team1 = value;
        }
    }

    public GameObject[] Team2
    {
        get
        {
            return team2;
        }

        set
        {
            team2 = value;
        }
    }
    #endregion

    public void OnConvertArray(GameObject[] team1, GameObject[] team2)
    {
        if(unitsPrefab != null)
        {
            for (int i = 0, l = unitsPrefab.Length; i < l; ++i)
            {
                for (int a = 0, f = team1.Length; a < f; ++a)
                {
                    if(unitsPrefab[i].gameObject.GetComponent<UnitCara>().unitStats == team1[a].gameObject.GetComponent<UnitStatsButtons>().stats)
                    {
                        Team_1_AsInt[a] = i;
                    }
                }
                for (int a = 0, f = team2.Length; a < f; ++a)
                {
                    if (unitsPrefab[i].gameObject.GetComponent<UnitCara>().unitStats == team2[a].gameObject.GetComponent<UnitStatsButtons>().stats)
                    {
                        Team_2_AsInt[a] = i;
                    }
                }
            }
        }
    }


    private void Awake()
    {
        Debug.Log("Test : " + Team_1_AsInt[0]);
    }
}
