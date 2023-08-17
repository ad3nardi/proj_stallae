using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Cursor Settings")]
public class so_cursorHolder : ScriptableObject
{

    [SerializeField] public Texture2D _defaultCursor;
    [SerializeField] public Texture2D _crosshairCursor;

}
