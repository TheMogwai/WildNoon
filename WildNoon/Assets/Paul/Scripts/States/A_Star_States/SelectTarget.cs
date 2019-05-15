using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine.EventSystems;

public class SelectTarget : IState
{
    TurnBasedManager m_TurnBaseManager;
    public SelectTarget(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.Select();
        m_TurnBaseManager.OnUnitSelected();
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
		m_TurnBaseManager.HandleButtonUnderRay(m_TurnBaseManager.Ray);
        m_TurnBaseManager.RayToNodes();
        if (m_TurnBaseManager.UnitUnderMouse != null)
        {
            if (m_TurnBaseManager.UnitUnderMouse.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<UnitCara>().IsTeam2)
            {
                GetOutOfState();
            }
        }
    }

    void GetOutOfState()
    {
        m_TurnBaseManager.ChangeState(2);
    }



}
