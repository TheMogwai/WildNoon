using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class Jacky_TpRange : IState
{

    TurnBasedManager m_TurnBaseManager;
    public Jacky_TpRange(TurnBasedManager turnBaseManager)
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
        HandleButtonUnderRayTPRange(m_TurnBaseManager.Ray);

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
    public void HandleButtonUnderRayTPRange(Ray ray)
    {
        var button = m_TurnBaseManager.GetByRay<Astar3DButton>(ray);

        if (m_TurnBaseManager.EventSystem.IsPointerOverGameObject())
        {
            return;

        }
        else if (button != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (m_TurnBaseManager.Player.OnActiveUnit1.ActionPoints > 0)
            {
                m_TurnBaseManager.Player.OnCoolDownspell();
                m_TurnBaseManager.Player.OnCoolDownDisplay(1);
                m_TurnBaseManager.ChangeState(0);
                m_TurnBaseManager.StartCoroutine(m_TurnBaseManager.TpToNode(m_TurnBaseManager.Selected, button.node));
            }
        }
    }
}
