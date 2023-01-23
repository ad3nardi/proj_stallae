using JetBrains.Annotations;
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
    public float unitAttackRange;
    public float unitFireRate;
    public List<unit_subsytems> unitSubSystems;

    [Header("Movement Settings")]
    public float unitMaxSpeed;
    public float unitAcceleration;
    public float unitRotationSpeed;
    public float unitSlowdownTime;
    public float unitWallForce;
    public float unitWallDist;
    public float unitEndReachedDistance;
    public float unitCheckDistance;
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
