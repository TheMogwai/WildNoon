using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackyTheKid : UnitCara
{
    public override void OnUsingSpell(Spells Spell, int i)
    {
        base.OnUsingSpell(Spell, i);
        switch (i)
        {
            case 0:
                break;
            case 1:
                Player.TurnBasedManager.ChangeState(5);
                break;
            case 2:
                Player.TurnBasedManager.ChangeState(4);
                break;
            case 3:
                Player.TurnBasedManager.ChangeState(7);
                break;
            default:
                break;
        }

    }

    public override void OnUnitPassiveEffect()
    {
        if(ArmorPoint + Spells1[0].m_armorBonus < unitStats.m_armor)
        {
            ArmorPoint += Spells1[0].m_armorBonus;
        }
        else
        {

            ArmorPoint = unitStats.m_armor;
        }
    }

    public override void AutoAttack(UnitCara target)
    {
        StartCoroutine(AutoAttackTimer(Player._onActiveUnit, target));
    }

    public IEnumerator AutoAttackTimer(UnitCara unit, UnitCara target)
    {
        if (!target.m_isInAnimation && Player._onActiveUnit.ActionPoints > 0)
        {
            Player._onActiveUnit.m_isInAnimation = true;
            Player._onActiveUnit.ActionPoints = Player._onActiveUnit.ActionPoints - Player._onActiveUnit.AutoAttackCost;
            Player.ActionPointsDisplay(Player._onActiveUnit.ActionPoints);

            yield return new WaitForSeconds(0.5f);                                //Temps de l'anim de l'attaque
            Player._onActiveUnit.m_isInAnimation = false;
            if (target != null)
            {
                target.OnTakingDamage(unit.Damage);
            }
        }
    }
}
