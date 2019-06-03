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
    [Header("Artwork Team 1")]
    public Sprite characterIsFirstArtwork;
    public Sprite characterIsLastArtwork;
    public Sprite characterIsNeitherArtwork;
    [Space]
    [Header("Artwork Team 2")]
    public Sprite characterIsFirstArtwork2;
    public Sprite characterIsLastArtwork2;
    public Sprite characterIsNeitherArtwork2;
    [Space]
    [Header("Unit Stats")]
    public int m_courage;
    public int m_heatlh;
    public int m_armor;
    public int m_damage;
    public int m_range;
    public int m_mobility;
    /*[Space]
    [Header("Stats Max")]
    public int m_maxCourage;
    public int m_maxHeatlh;
    public int m_maxArmor;
    public int m_maxDamage;
    public int m_maxRange;
    public int m_maxMobility;*/
    [Space]
    public int m_autoAttackCost;
    [Space]
    [Header("Unit Spells")]
    public Spells firstSpell;
    public Spells secondSpell;
    public Spells thirdSpell;
    public Spells FourthSpell;

}
