using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Assets/Battlegroup")]
public class enemBattlegroup_settings : ScriptableObject
{
    [Header("Battlegroup Units")]
    public List<GameObject> _unitsList = new List<GameObject>();

    [Header("Behaviour Settings")]
    public float _aggro;
    public float _spread;

    [Header("Settings Front Zone Check")]
    public int _enemCountZone;
    public Vector3 _zoneOffsetPos, _zonetOffsetRot, _zoneOffsetScale;

    [Header("Settings Flank Check")]
    public float _flankCheckSize;

    [Header("Settings Range Check")]
    public LayerMask _checkLayer;
    public float _checkRangeBandOne, _checkRangeBandTwo, _checkRangeBandThree;
    public float _enemFriendsCheckRange;
    public float _enemIsolationCheckRange;

    [Header("Spread Settings")]
    public float _spreadRadius;

    [Header("Attack Target Choice")]
    public float _targetWeight;

    [Header("Route Settings")]
    public float _toCommonPoint, _toFarPoint, _toFleet;

    [Header("Distance Engagement Settings")]
    public float _squadronns;
    public float _outerRange;
    public float _targets;

    [Header("Threshold Inputs")]
    public float _range;
    public float _hp;
    public float _pwr;
    public float _enemDist;
    public float _enemCountNear;
    public float _enemCountFar;
    public float _enemCountMid;
    public float _enemCountClose;
    public float _distFleet;
    public float _distObj;
    public float _strVwk;

    [Header("Threshold Adjustments")]
    public float _adjEnemNear;
    public float _adjEnemIsolate;
    public float _adjEnemFriends;

    [Header("Thresholds")]
    public float _threshEngage;
    public float _threshEnemNear;
    public float _threshEnemIsolate;
    public float _threshEnemFriends;
}
