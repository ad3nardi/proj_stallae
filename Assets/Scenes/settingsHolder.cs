using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsHolder: OptimizedBehaviour
{
    [SerializeField] public database_settings DBSettings;

    private static pm_settings _pmSettings;
    [SerializeField] public static pm_settings PMSettings
    {
        get
        {
            if(_pmSettings == null)
            {
                _pmSettings = ScriptableObject.CreateInstance<pm_settings>();
            }
            return _pmSettings;
        }
    }

}
