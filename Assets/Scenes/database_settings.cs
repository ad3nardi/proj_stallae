using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Settings Database")]
public class database_settings : ScriptableObject
{
    [Header("Settings")]
    public pm_settings _pmSettings;

}
