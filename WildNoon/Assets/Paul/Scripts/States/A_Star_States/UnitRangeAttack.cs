using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class UnitRangeAttack : IState
{
    Vector3 _heading;
    float distanceToPlayer;
    TurnBasedManager m_TurnBaseManager;
    float range;
    public UnitRangeAttack(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.OnUnitAttack();
        range = m_TurnBaseManager.Player.OnActiveUnit1.Range * m_TurnBaseManager.nodes;
    }


    public void FixedUpdate()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !m_TurnBaseManager.Player.OnActiveUnit1.m_isInAnimation && m_TurnBaseManager.UnitUnderMouse != null)
        {
            var heading = m_TurnBaseManager.UnitUnderMouse.gameObject.transform.position - m_TurnBaseManager.Player.OnActiveUnit1.gameObject.transform.position;
            _heading = heading;
            distanceToPlayer = heading.magnitude;

            if(distanceToPlayer < range)
            {
                m_TurnBaseManager.AutoAttack(m_TurnBaseManager.UnitUnderMouse.GetComponent<UnitCara>());
            }
        }
        if(m_TurnBaseManager.UnitUnderMouse == null)
        {
            GetOutOfState();
        }
        else if (m_TurnBaseManager.UnitUnderMouse.GetComponent<UnitCara>().IsTeam2 == m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<UnitCara>().IsTeam2)
        {
            GetOutOfState();
        }
    }

    public void Exit()
    {
    }
    void GetOutOfState()
    {
        m_TurnBaseManager.ChangeState(0);
    }
}
