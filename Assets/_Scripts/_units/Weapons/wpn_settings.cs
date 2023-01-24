using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName="Units/Weapon Settings")]
public class wpn_settings : ScriptableObject
{
    [Header("Weapon Attributes")]
    public WeaponType weapon;

    [Header("Weapon Settings")]
    public float weapon_damage;
    public float weapon_fireSpeed;
    public float weapon_reloadTime;
    public VisualEffect weapon_visual;

    
}
public enum WeaponType
{
    //Unguided
    Laser,
    TurboLaser,
    //UnguidedProjectile
    Torpedo,
    //Guided
    Missile,
    Ion
}

