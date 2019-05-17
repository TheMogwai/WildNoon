﻿using System.Collections;
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
    }
}
