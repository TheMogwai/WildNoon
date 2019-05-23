using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class Jacky_TauntRange : IState
{

    TurnBasedManager m_TurnBaseManager;
    public Jacky_TauntRange(TurnBasedManager turnBaseManager)
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
        
    }

    public void Exit()
    {
    }


    void GetOutOfState()
    {
    }
    
}
