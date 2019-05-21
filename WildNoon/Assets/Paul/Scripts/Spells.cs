using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell/Actif")]
public class Spells : ScriptableObject {


    public int cost;
    public int coolDown;
    [Space]
    public Sprite artwork;
    [Space]
    public int m_courageBonus;
    public int m_heatlhBonus;
    public int m_armorBonus;
    public int m_damageBonus;
    public int m_rangeBonus;
    public int m_mobilityBonus;
    [Space]
    public int m_spellDamage;
    public int m_spellRange;
    public int m_spellAOE;
    [Space]
    public int m_courageMalus;
    public int m_heatlhMalus;
    public int m_armorMalus;
    public int m_damageMalus;
    public int m_rangeMalus;
    public int m_mobilityMalus;
    [Space]
    [TextArea(1, 20)]
    public string spell_Description;

}

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell/Passif")]
public class SpellsPassif : ScriptableObject
{

    public Sprite artwork;
    public string spell_Description;

}


