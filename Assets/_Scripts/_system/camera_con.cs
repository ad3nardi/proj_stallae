using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class camera_con : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private CinemachineVirtualCamera _cvc;
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform camTrans;
    [SerializeField] private PlayerInput _playerInput;

    [Header("Inputs")]
    [SerializeField] private InputAction camDragPos;
    [SerializeField] private InputAction camMovementAct;
    [SerializeField] private InputAction camRotateAct;
    [SerializeField] private InputAction camRotateButtonAct;
    [SerializeField] private InputAction camZoomAct;
    [SerializeField] private InputAction camDragAct;

    [Header("Framing")]
    [SerializeField] private bool _framelock;
    [SerializeField] private OptimizedBehaviour _framedObject;

    [Header("Horizontal Motion")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _damping;
    [SerializeField] private bool _draggingCam;

    [Header("Vertical/Zoom Motion")]
    [SerializeField] private float _stepSize;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _zoomDampening;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] [Range(0.05f, 200f)] private float _zoomSense;

    [Header("Rotation")]
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private bool _rotatingCam;

    [Header("Screen Edge Motion")]
    [SerializeField] [Range(0f, 0.1f)] private float edgeTolerance;
    [SerializeField] private bool _enableScreenEdge;

    [Header("Max Positions")]
    [SerializeField] private float _maxXpos;
    [SerializeField] private float _maxYpos;

    //DNT = do not touch
    [Header("- DNT - Dynamic")]
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private float _zoomHeight;

    [Header("- DNT - Track velocity w/o RB")]
    [SerializeField] private Vector3 _horizontalVelocity;
    [SerializeField] private Vector3 _lastPosition;

    [Header("- DNT - Where drag actoin started")]
    [SerializeField] private Vector3 _startDrag;

    //Unity Functions
    private void Awake()
    {
        _cam = Helpers.Camera;
        _cvc = GetComponentInChildren<CinemachineVirtualCamera>();
        camTrans = _cvc.GetComponent<Transform>();
        _playerInput.GetComponent<PlayerInput>();

        _framelock = false;
    }

    private void OnEnable()
    {
        _lastPosition = CachedTransform.position;
        //Attatch Input Actions
        camMovementAct      = _playerInput.actions["cameraMove"];
        camRotateAct        = _playerInput.actions["cameraRotate"];
        camRotateButtonAct  = _playerInput.actions["cameraRotateButton"];
        camZoomAct          = _playerInput.actions["cameraZoom"];
        camDragAct          = _playerInput.actions["cameraDrag"];
        camDragPos          = _playerInput.actions["cameraDragPos"];

        //Subscribe to Input Events
        camRotateAct.performed += rotateCamera;
        camRotateButtonAct.performed += rotateCameraButtonInp;
        camRotateButtonAct.canceled += rotateCameraButtonInp;
        camZoomAct.performed += zoomCameraInp;
        camDragAct.performed += dragCameraInp;
        camDragAct.canceled += dragCameraInp;

        //Inital Value
        _zoomHeight = camTrans.localPosition.y;
        camTrans.LookAt(CachedTransform);
    }

    private void OnDisable()
    {
        camRotateAct.performed -= rotateCameraButtonInp;
        camRotateButtonAct.performed -= rotateCameraButtonInp;
        camRotateButtonAct.canceled -= rotateCameraButtonInp;
        camZoomAct.performed -= zoomCameraInp;
        camDragAct.performed -= dragCameraInp;
        camDragAct.canceled -= dragCameraInp;

    }

    private void Update()
    {
        if (_enableScreenEdge)
            CheckMouseAtScreenEdge();

        DragCamera();
        UpdateVelocity();
        UpdateCameraPosition();

        if (_framelock)
        {
            UpdateFramePosition();
            return;
        }
        GetKeyboardMovement();
        
        UpdateBasePosition();
        UpdateBounds();
    }

    //UPDATE FUNCTONS
    private void UpdateFramePosition()
    {
        if(_framedObject!= null)
        {
            CachedTransform.position = _framedObject.CachedTransform.position;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(camTrans.localPosition.x, _zoomHeight, camTrans.localPosition.z);
        zoomTarget -= _zoomSpeed * (_zoomHeight - camTrans.localPosition.y) * Vector3.forward;

        camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, zoomTarget, Time.deltaTime * _zoomDampening);
        camTrans.LookAt(CachedTransform);
    }
    private void UpdateVelocity()
    {
        _horizontalVelocity = (CachedTransform.position - _lastPosition) / Time.deltaTime;
        _horizontalVelocity.y = 0;
        _lastPosition = CachedTransform.position;
    }
    private void GetKeyboardMovement()
    {
        Vector3 inpVal = camMovementAct.ReadValue<Vector2>().x * GetCameraRight()
            + camMovementAct.ReadValue<Vector2>().y * GetCameraForward();

        inpVal = inpVal.normalized;

        if (inpVal.sqrMagnitude > 0.1f)
            _targetPos += inpVal;

    }
    private void UpdateBasePosition()
    {
        if (_targetPos.sqrMagnitude > 0.1f)
        {
            _speed = Mathf.Lerp(_speed, _maxSpeed, Time.deltaTime * _acceleration);
            CachedTransform.position += _targetPos * _speed * Time.deltaTime;
        }
        else
        {
            _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _damping);
            CachedTransform.position += _horizontalVelocity * Time.deltaTime;
        }

        _targetPos = Vector3.zero;
    }
    private void UpdateBounds()
    {
        if (CachedTransform.position.x < -_maxXpos)
        {
            CachedTransform.position = new Vector3(-_maxXpos, 0, CachedTransform.position.z);
        }

        if (CachedTransform.position.x > _maxXpos)
        {
            CachedTransform.position = new Vector3(_maxXpos, 0, CachedTransform.position.z);
        }

        if (CachedTransform.position.z < -_maxYpos)
        {
            CachedTransform.position = new Vector3(CachedTransform.position.x, 0, -_maxYpos);
        }

        if (CachedTransform.position.z > _maxYpos)
        {
            CachedTransform.position = new Vector3(CachedTransform.position.x, 0, _maxYpos);
        }
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePos = camDragPos.ReadValue<Vector2>();
        Vector3 moveDirection = Vector3.zero;

        if (mousePos.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePos.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePos.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePos.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        _targetPos += moveDirection;
    }

    //CAMERA ORIENTATION
    private Vector3 GetCameraRight()
    {
        Vector3 right = camTrans.right;
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = camTrans.forward;
        forward.y = 0;
        return forward;
    }

    //Camera Dragging
    private void dragCameraInp(InputAction.CallbackContext context)
    {
        FrameUnlock();
        _draggingCam = context.ReadValueAsButton();
    }
    private void DragCamera()
    {
        if (!_draggingCam)
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _cam.ScreenPointToRay(camDragPos.ReadValue<Vector2>());

        if(plane.Raycast(ray, out float distance))
        {
            if (camDragAct.WasPerformedThisFrame())
                _startDrag = ray.GetPoint(distance);
            else
                _targetPos += _startDrag - ray.GetPoint(distance);
        }
    }

    //Camera Rotating
    private void rotateCameraButtonInp(InputAction.CallbackContext context)
    {
        _rotatingCam = context.ReadValueAsButton();
    }

    private void rotateCamera(InputAction.CallbackContext context)
    {
        if (!_rotatingCam)
            return;

        float value = context.ReadValue<Vector2>().x;
        CachedTransform.rotation = Quaternion.Euler(0f, value * _maxRotationSpeed + CachedTransform.rotation.eulerAngles.y, 0f);
    }

    //Camera Zoom
    private void zoomCameraInp(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<Vector2>().y / _zoomSense;
        if(Mathf.Abs(value) > 0.1f)
        {
            _zoomHeight = camTrans.localPosition.y + value * _stepSize;
            if (_zoomHeight < _minHeight)
                _zoomHeight = _minHeight;
            else if (_zoomHeight > _maxHeight)
                _zoomHeight = _maxHeight;
        }
    }

    public void FrameCamera(OptimizedBehaviour ob)
    {
        _framelock = true;
        _framedObject = ob;    
    }

    public void FrameFreeCamera()
    {
        _framelock = true;
        if(SelectionMan.Instance.AvaliableUnits.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, SelectionMan.Instance.AvaliableUnits.Count);
            _framedObject = SelectionMan.Instance.AvaliableUnits[rand];
        }
    }
    public void FrameValeria()
    {

    }
    public void FrameUnlock()
    {
        _framelock = false;
        _framedObject = null;

    }
}