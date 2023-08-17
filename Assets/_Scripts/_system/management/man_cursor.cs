using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class man_cursor : Singleton<man_cursor>
{
    [SerializeField] private so_cursorHolder _cursors;
    public void Awake()
    {
        _cursors = Resources.Load<so_cursorHolder>("UI_Sprites/cursorsDefault");
    }

    public void ActivateMainCursor()
    {
        Cursor.SetCursor(_cursors._defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    public void ActivateCrosshairCursor()
    {
        Cursor.SetCursor(_cursors._crosshairCursor, new Vector2(_cursors._crosshairCursor.width/2, _cursors._crosshairCursor.height/2), CursorMode.Auto);
    }

    //man_cursor.instance.activate-xxxCursor-

}
