using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell/Actif")]
public class Spells : ScriptableObject {

    public int cost;
    public int coolDown;
    public Sprite artwork;
    [TextArea(1, 20)]
    public string spell_Description;

}

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell/Passif")]
public class SpellsPassif : ScriptableObject
{

    public Sprite artwork;
    public string spell_Description;

}


