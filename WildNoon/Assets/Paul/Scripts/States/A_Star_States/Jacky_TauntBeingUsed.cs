using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding.Examples;

public class Jacky_TauntBeingUsed : IState
{

    Vector3 _heading;
    float distanceToPlayer;
    float range;
    List<UnitCara> unitInRange;

    TurnBasedManager m_TurnBaseManager;
    public Jacky_TauntBeingUsed(TurnBasedManager turnBaseManager)
    {
        m_TurnBaseManager = turnBaseManager;
    }


    public void Enter()
    {

        m_TurnBaseManager.DestroyPossibleMoves();

        CheckCollision(m_TurnBaseManager.Player._onActiveUnit, m_TurnBaseManager.Player._onActiveUnit.transform.position, (m_TurnBaseManager.Player._onActiveUnit.unitStats.FourthSpell.m_spellRange) * m_TurnBaseManager.nodes);

        m_TurnBaseManager.Player.OnCoolDownspell();
        m_TurnBaseManager.Player.OnCoolDownDisplay(3);
        m_TurnBaseManager.StartCoroutine(m_TurnBaseManager.TauntAttack(m_TurnBaseManager.Selected, unitInRange));
        m_TurnBaseManager.ChangeState(0);


        
        /* Collider[] = 

         var heading = m_TurnBaseManager.UnitUnderMouse.gameObject.transform.position - m_TurnBaseManager.Player.OnActiveUnit1.gameObject.transform.position;
         _heading = heading;
         distanceToPlayer = heading.magnitude;

             else if (unit.gameObject.GetComponent<UnitCara>().IsTeam2 != m_TurnBaseManager.Player.OnActiveUnit1.GetComponent<UnitCara>().IsTeam2 && Input.GetKeyDown(KeyCode.Mouse0))
             {

                 if (m_TurnBaseManager.Player.OnActiveUnit1.ActionPoints > 0)
                 {
                     if (distanceToPlayer < range)
                     {
                         m_TurnBaseManager.Player.OnCoolDownspell();
                         m_TurnBaseManager.Player.OnCoolDownDisplay(2);
                         m_TurnBaseManager.ChangeState(0);
                         m_TurnBaseManager.StartCoroutine(m_TurnBaseManager.SlowAttack(m_TurnBaseManager.Selected, unit));
                     }
                 }
             }*/

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

    public void CheckCollision(UnitCara unit,Vector3 centre, float range)
    {
        Collider[] hitCollider = Physics.OverlapSphere(centre, range);
        unitInRange = new List<UnitCara>();
        for (int i = 0, l = hitCollider.Length; i < l; ++i)
        {
            if(hitCollider[i].GetComponentInParent<UnitCara>() != null)
            {
                if (hitCollider[i].GetComponentInParent<UnitCara>().IsTeam2 != unit.IsTeam2)
                {
                    unitInRange.Add(hitCollider[i].GetComponentInParent<UnitCara>());
                }
            }
        }
    }

}
