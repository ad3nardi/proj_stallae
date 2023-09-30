using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName ="Units/Unit Settings")]
public class unit_settings : ScriptableObject
{
    [Header("Unit Identifier")]
    public int id;

    [Header("Unit Attributes")]
    public FactionType faction;
    public SizeTag sizeTag;
    public bool canAttack;
    public bool _isSquadron;
    public bool hasSubSystems;
    public bool hasShields;

    [Header("Unit Stats")]
    public float unitPointCost;
    public float unitMaxHitPoints;
    public float untMaxShields;
    public float unitAttackRange;
    public float unitFireRate;
    public List<unit_subsystem> unitSubSystems;

    [Header("Movement Settings")]
    public float unitRadius;
    public float unitHeight;
    public float unitMaxSpeed;
    public float unitAcceleration;
    public float unitRotationSpeed;
    public float unitSlowdownTime;
    public float unitWallForce;
    public float unitWallDist;
    public float unitEndReachedDistance;
    public float unitCheckDistance;

    [Header("Combat and Ability Settings")]
    public float abCooldownTime;
    public float abActiveTime;
    public bool abNeedsTarget;

    [Header("Combat Damage")]
    public OptimizedBehaviour debris;
    public OptimizedBehaviour debrisHull;
    public OptimizedBehaviour vfx_subExplosion;
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
