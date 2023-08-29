using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    public static Camera _camera;
    public static LayerSet _layerSet;
    public static TagSet _tagSet;
    public static EventSystem _eventSystem;

    public static Camera Camera
    {
        get { 
            if(_camera == null )_camera= Camera.main;
            return _camera;
        }
    }
    public static EventSystem EventSystem
    {
        get
        {
            if (_eventSystem == null) _eventSystem = EventSystem.current;
            return _eventSystem;
        }
    }
    public static LayerSet LayerSet
    {
        get
        {
            if(_layerSet == null) _layerSet = ScriptableObject.CreateInstance<LayerSet>();
            return _layerSet;
        }
    }
    public static TagSet TagSet
    {
        get
        {
            if (_tagSet == null) _tagSet = ScriptableObject.CreateInstance<TagSet>();
            return _tagSet;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    } 
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
        //Set the transform.position of an object to the RectTransform to follow the rect transform in world coord
    }
    public static void DeleteChildren(this Transform t)
    {
        foreach(Transform child in t) Object.Destroy(child.gameObject);
    }

    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f)
        {
            return 1.0f;
        }
        else if (dir < 0.0f)
        {
            return -1.0f;
        }
        else
        {
            return 1.0f;
        }
    }

    public static Vector3 radialPoints(List<unit_Manager> units, Vector3 hitPoint, int i)
    {
        float radsum = 0;
        for (int r = 0; r < units.Count; r++)
        {
            //Radius of object
            //radsum += units[r].transform.GetComponent<RichAI>().radius;
        }

        float radius = radsum / (Mathf.PI);
        radius *= 2f;

        float deg = 2 * Mathf.PI * i / units.Count;
        Vector3 p = hitPoint + new Vector3(Mathf.Cos(deg), 0, Mathf.Sin(deg)) * radius;

        return p;
    }
}
