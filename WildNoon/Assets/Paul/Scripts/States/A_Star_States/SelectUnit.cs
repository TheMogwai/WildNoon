using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class SelectUnit : IState
{

    TurnBasedManager m_TurnBaseManager;
    public SelectUnit(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.DestroyPossibleMoves();
        m_TurnBaseManager.Select();
    }

    public void Exit()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (m_TurnBaseManager.Player != null)
            {
                if (m_TurnBaseManager.UnitUnderMouse == m_TurnBaseManager.Player._onActiveUnit.GetComponent<TurnBasedAI>())
                {
                    m_TurnBaseManager.ChangeState(1);
                }
            }
            else
            {
                m_TurnBaseManager.ChangeState(1);
            }
        }
        else
        {
            if (m_TurnBaseManager.UnitUnderMouse != null)
            {
                if (m_TurnBaseManager.UnitUnderMouse.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player._onActiveUnit.GetComponent<UnitCara>().IsTeam2)
                {
                    GetOutOfState();
                }
            }
        }

        
    }
    void GetOutOfState()
    {
        m_TurnBaseManager.ChangeState(2);
    }
}
