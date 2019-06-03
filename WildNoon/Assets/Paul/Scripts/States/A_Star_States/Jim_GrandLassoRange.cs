using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class Jim_GrandLassoRange : IState
{

    TurnBasedManager m_TurnBaseManager;
    public Jim_GrandLassoRange(TurnBasedManager turnBaseManager)
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
