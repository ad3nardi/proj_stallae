using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TagSet", menuName = "TagSet")]
public class TagSet : ScriptableObject
{
    [Header("Size Tag")]
    public int tagSquadron = 0;
    public int tagSmalll = 1;
    public int tagMedium = 2;
    public int tagLarge = 3;
    public int tagHuge = 4;
    public int tagStation = 5;
    public int tagProjectile = 6;
}
public enum SizeTag
{
    tagSquadron,
    tagSmalll,
    tagMedium,
    tagLarge,
    tagHuge,
    tagStation,
    tagProjectile
}