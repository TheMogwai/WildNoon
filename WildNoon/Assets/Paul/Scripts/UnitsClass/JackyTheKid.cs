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
                print("Grog SMASH!");
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
}
