using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class man_cursor : MonoBehaviour
{
    public static man_cursor _instance;

    [SerializeField] private Texture2D _cursorTexMain, _cursorTexCrosshair;

    private void Awake()
    {
        _instance = this;
    }

    public void ActivateMainCursor()
    {
        Cursor.SetCursor(_cursorTexMain, Vector2.zero, CursorMode.Auto);
    }
    public void ActivateCrosshairCursor()
    {
        Cursor.SetCursor(_cursorTexMain, new Vector2(_cursorTexCrosshair.width/2, _cursorTexCrosshair.height/2), CursorMode.Auto);
    }

    //man_cursor.instance.activate-xxxCursor-

}
