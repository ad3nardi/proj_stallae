using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Units/Unit Settings")]
public class unit_settings : ScriptableObject
{
    [Header("Unit Identifier")]
    public int id;

    [Header("Unit Attributes")]
    public FactionType faction;
    public bool hasShields;
    public bool canAttack;
    public bool hasSubSystems;

    [Header("Unit Stats")]
    public float unitPointCost;
    public float unitMaxHitPoints;
    public float untMaxShields;
    public float unitMoveSpeed;
    public float unitAttackRange;
    public float unitFireRate;
    public List<unit_subsytems> unitSubSystems;
}

public enum FactionType
{
    FF,
    MM,
    AD,
    RB,
    AR,
    PHB,
    TC
}
