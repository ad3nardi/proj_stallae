using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class man_cursor : Singleton<man_cursor>
{
    [SerializeField] private Texture2D _cursorTexMain, _cursorTexCrosshair;
    public void Awake()
    {
        _cursorTexMain = Resources.Load<Texture2D>("UI_Sprites/curs_default");
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
