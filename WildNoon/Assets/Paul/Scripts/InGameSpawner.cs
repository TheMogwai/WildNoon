using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSpawner : MonoBehaviour
{
    GameObject m_teams;

    public GameObject[] Units;

    public GameObject[] SpawnerTeam_1;
    public GameObject[] SpawnerTeam_2;

    public Transform m_Team_1_Root;
    public Transform m_Team_2_Root;
    [Space]
    [Header("Debug sans avoir à créer de team")]
    public bool debug;

    private void Awake()
    {
        if (!debug)
        {
            
            m_teams = FindObjectOfType<Unit_Spawer>().gameObject;
            for (int i = 0, l = SpawnerTeam_1.Length; i < l; ++i)
            {
                //Les units doient avoir leur pivot à leurs pieds pour spawn sur les spawer au sol (et pas à 0.5 du sol ;))
                Instantiate(Units[m_teams.GetComponent<Unit_Spawer>().Team_1_AsInt[i]], SpawnerTeam_1[i].transform.position, Quaternion.identity, m_Team_1_Root);
                Instantiate(Units[m_teams.GetComponent<Unit_Spawer>().Team_2_AsInt[i]], SpawnerTeam_2[i].transform.position, Quaternion.identity, m_Team_2_Root);
            }
        }
        else
        {
            for (int i = 0, l = SpawnerTeam_1.Length; i < l; ++i)
            {
                Instantiate(Units[0], SpawnerTeam_1[i].transform.position, Quaternion.identity, m_Team_1_Root);
                Instantiate(Units[1], SpawnerTeam_2[i].transform.position, Quaternion.identity, m_Team_2_Root);
            }
        }
    }
}
