using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine.EventSystems;

public class Jacky_SlowRange : IState
{
    TurnBasedManager m_TurnBaseManager;
    public Jacky_SlowRange(TurnBasedManager turnBaseManager)
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
        HandleButtonUnderRaySlowRange(m_TurnBaseManager.Ray);

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

    public void HandleButtonUnderRaySlowRange(Ray ray)
    {
        var unit = m_TurnBaseManager.GetByRay<UnitCara>(ray);
        if(unit != null)
        {
            if (m_TurnBaseManager.EventSystem.IsPointerOverGameObject())
            {
                return;

            }
            else if (unit.gameObject.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<UnitCara>().IsTeam2 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (m_TurnBaseManager.UnitActionPoints1 > 0)
                {
                    m_TurnBaseManager.Player.OnCoolDownspell();
                    m_TurnBaseManager.Player.OnCoolDownDisplay(2);
                    m_TurnBaseManager.ChangeState(0);
                    m_TurnBaseManager.StartCoroutine(m_TurnBaseManager.SlowAttack(m_TurnBaseManager.Selected, unit));
                }
            }
        }
    }
}
