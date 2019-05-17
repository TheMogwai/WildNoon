﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class UnitRangeAttack : IState
{

    TurnBasedManager m_TurnBaseManager;
    public UnitRangeAttack(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.OnUnitAttack();
    }


    public void FixedUpdate()
    {
    }

    public void Update()
    {
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
        m_TurnBaseManager.ChangeState(1);
    }
}
