using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimLasso : UnitCara
{


    UnitCara OnSpotted;

    public override void OnUsingSpell(Spells Spell, int i)
    {
        base.OnUsingSpell(Spell, i);
        switch (i)
        {
            case 0:
                break;
            case 1:
                Player.TurnBasedManager.ChangeState(9);
                break;
            case 2:
                Player.TurnBasedManager.ChangeState(8);
                break;
            case 3:
                Player.TurnBasedManager.ChangeState(11);
                break;
            default:
                break;
        }

    }

    public override void OnUnitPassiveEffect()
    {

    }

    public override void AutoAttack(UnitCara target)
    {
        if (target.JimPassifEffect)
        {
            StartCoroutine(AutoAttackTimer(Player._onActiveUnit, target, Player._onActiveUnit.unitStats.firstSpell.m_damageBonus));
        }
        else
        {
            StartCoroutine(AutoAttackTimer(Player._onActiveUnit, target, 0));
            
        }
    }

    public IEnumerator AutoAttackTimer(UnitCara unit, UnitCara target, int damage)
    {
        if (!target.m_isInAnimation && unit.ActionPoints > 0)
        {
            target.JimPassifEffect = true;
            if (OnSpotted == null)
            {
                OnSpotted = target;
            }
            else
            {
                OnSpotted.JimPassifEffect = false;
                OnSpotted = target;
            }
            unit.m_isInAnimation = true;
            unit.ActionPoints = unit.ActionPoints - unit.AutoAttackCost;
            Player.ActionPointsDisplay(Player._onActiveUnit.ActionPoints);

            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
            unit.m_isInAnimation = false;
            if (target != null)
            {
                int damageBonus = unit.Damage + damage;
                target.OnTakingDamage(damageBonus);
            }
        }
    }
}