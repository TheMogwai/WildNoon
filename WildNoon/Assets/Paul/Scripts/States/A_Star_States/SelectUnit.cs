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
                if (m_TurnBaseManager.UnitUnderMouse == m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<TurnBasedAI>())
                {
                    m_TurnBaseManager.ChangeState(1);
                }
            }
            else
            {
                m_TurnBaseManager.ChangeState(1);
            }
        }
    }
}
