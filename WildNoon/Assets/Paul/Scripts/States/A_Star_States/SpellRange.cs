using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class SpellRange : IState
{

    TurnBasedManager m_TurnBaseManager;
    public SpellRange(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.OnShowRange();
    }


    public void FixedUpdate()
    {
    }

    public void Update()
    {
        m_TurnBaseManager.HandleButtonUnderRaySpellRange(m_TurnBaseManager.Ray);

        if (Input.GetKeyDown(KeyCode.Escape))
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
