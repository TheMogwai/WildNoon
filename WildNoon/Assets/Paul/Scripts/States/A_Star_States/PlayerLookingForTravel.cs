using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine.EventSystems;

public class PlayerLookingForTravel : IState
{
    TurnBasedManager m_TurnBaseManager;
    public PlayerLookingForTravel(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
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
