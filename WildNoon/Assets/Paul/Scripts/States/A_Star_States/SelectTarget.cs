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
        m_TurnBaseManager.OnUnitSelected();
    }

    public void Exit()
    {
        m_TurnBaseManager.Player.MovementCost(0, false);
    }

    public void FixedUpdate()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(m_TurnBaseManager.UnitUnderMouse ==null && m_TurnBaseManager.NodeUnderMouse == null)
            {
                GetOutOfState();
            }
            else
            {

            }
        }
        m_TurnBaseManager.HandleButtonUnderRay(m_TurnBaseManager.Ray);


        /*if (m_TurnBaseManager.UnitUnderMouse != null)
        {
            if (m_TurnBaseManager.UnitUnderMouse.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<UnitCara>().IsTeam2)
            {
                GetOutOfState();
            }
        }*/
    }

    void GetOutOfState()
    {
        m_TurnBaseManager.ChangeState(0);
    }



}
