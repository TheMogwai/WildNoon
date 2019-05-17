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
                JackyTeleport();
                break;
            case 2:
                print("Whadya want?");
                break;
            case 3:
                print("Grog SMASH!");
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }

    }

    public override void OnUnitPassiveEffect()
    {
        if(ArmorPoint + Spells1[0].m_armorBonus < unitStats.m_armor)
        {
            Debug.Log(name + " il lui reste : " + ArmorPoint);
            ArmorPoint += Spells1[0].m_armorBonus;
        }
        else
        {

            ArmorPoint = unitStats.m_armor;
            Debug.Log(name + " est full armure");
        }
    }


    void JackyTeleport()
    {
    }
}
