using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManager : MonoBehaviour
{
    public missionStructure _activeMission;


}
public enum missionType
{
    none,
    valeria,
    capPoint,
    capArea,
    defendObj,
    hunt,
    koth
}