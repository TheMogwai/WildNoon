using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    [Range(0.05f, 2)] [SerializeField] private float m_gizmosRadius = 0.25f;
	[SerializeField] private Color m_SpawnerTeam_1 = Color.red;
	[SerializeField] private Color m_SpawnerTeam_2 = Color.blue;

    public bool Team1;

    void OnDrawGizmos()
    {
        if(Team1)
        {
            Gizmos.color = m_SpawnerTeam_1;
            Gizmos.DrawSphere(transform.position, m_gizmosRadius);
        }
        else
        {
			Gizmos.color = m_SpawnerTeam_2;
			Gizmos.DrawSphere(transform.position, m_gizmosRadius);
        }
	}
}
