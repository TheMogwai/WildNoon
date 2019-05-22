using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Characters : ScriptableObject
{
    [Header("Unit Info")]
    public string m_name;
    public string m_punshLine;
    [Space]
    [Header("Artwork")]
    public Sprite characterArtwork;
    [Space]
    public Sprite characterIsFirstArtwork;
    public Sprite characterIsLastArtwork;
    public Sprite characterIsNeitherArtwork;
    [Space]
    [Header("Unit Stats")]
    public int m_courage;
    public int m_heatlh;
    public int m_armor;
    public int m_damage;
    public int m_range;
    public int m_mobility;
    [Space]
    public int m_autoAttackCost;
    [Space]
    [Header("Unit Spells")]
    public Spells firstSpell;
    public Spells secondSpell;
    public Spells thirdSpell;
    public Spells FourthSpell;

}
