using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine.EventSystems;

public class Jim_PoingAmericain : IState
{
    Vector3 _heading;
    float distanceToPlayer;
    float range;

    TurnBasedManager m_TurnBaseManager;
    public Jim_PoingAmericain(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {
        m_TurnBaseManager.OnShowRange();
        range = m_TurnBaseManager.Player._onActiveUnit.Range * m_TurnBaseManager.nodes;
    }


    public void FixedUpdate()
    {
    }

    public void Update()
    {
        HandleButtonUnderRaySlowRange(m_TurnBaseManager.Ray);

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //if (m_TurnBaseManager.UnitUnderMouse == null && m_TurnBaseManager.NodeUnderMouse == null)
            //{
                GetOutOfState();
            //}
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
            else if (unit.gameObject.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player._onActiveUnit.GetComponent<UnitCara>().IsTeam2 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                var heading = m_TurnBaseManager.UnitUnderMouse.gameObject.transform.position - m_TurnBaseManager.Player._onActiveUnit.gameObject.transform.position;
                _heading = heading;
                distanceToPlayer = heading.magnitude;
                if (m_TurnBaseManager.Player._onActiveUnit.ActionPoints > 0)
                {
                    if(distanceToPlayer < range)
                    {
                        m_TurnBaseManager.Player.OnCoolDownspell();
                        m_TurnBaseManager.Player.OnCoolDownDisplay(2);
                        m_TurnBaseManager.ChangeState(0);
                        m_TurnBaseManager.StartCoroutine(m_TurnBaseManager.PoingAmericain(m_TurnBaseManager.Selected, unit));
                    }
                }
            }
        }
    }
}
