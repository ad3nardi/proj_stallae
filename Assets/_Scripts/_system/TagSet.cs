using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TagSet", menuName = "TagSet")]
public class TagSet : ScriptableObject
{
    [Header("Size Tag")]
    public int tagProjectile = 0;
    public int tagSquadron = 1;
    public int tagSmalll = 2;
    public int tagMedium = 3;
    public int tagLarge = 4;
    public int tagHuge = 5;
    public int tagStation = 6;
}
public enum SizeTag
{
    tagProjectile,
    tagSquadron,
    tagSmalll,
    tagMedium,
    tagLarge,
    tagHuge,
    tagStation
}