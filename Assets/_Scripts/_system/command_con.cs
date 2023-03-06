using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class command_con : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private LineRenderer _lineRender;
    [SerializeField] private float _maxDrag = 5f;
    [SerializeField] private Vector3 _dragStartPos;


    private void Awake()
    {
        _cam = Camera.main;
        _playerInput.GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        
    }

    public void DragStart(float x, float y)
    {
        _dragStartPos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));
        _lineRender.positionCount = 1;
        _lineRender.SetPosition(0, _dragStartPos);
    }
    public void Dragging(float x, float y)
    {
        Vector3 _draggingPos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));
        _lineRender.positionCount = 2;
        _lineRender.SetPosition(1, _draggingPos);

    }
    public void DragRelease(float x, float y)
    {
        _lineRender.positionCount = 0;
        Vector3 _dragReleasePos = _cam.ScreenToViewportPoint(new Vector3(x, y, 0));

        Vector3 force = _dragStartPos - _dragReleasePos;
        //Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

    }
}
