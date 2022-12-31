using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

[Serializable] public class InputSelectEvent : UnityEvent<bool> { }
[Serializable] public class InputSelectPosEvent : UnityEvent<float, float> { }
[Serializable] public class InputCamMoveEvent : UnityEvent<bool> { }
[Serializable] public class InputCamRotateEvent : UnityEvent<bool> { }

public class InputCon : MonoBehaviour
{
    /*
    [SerializeField] private InpActionMap inputAM;
    [Header("Input Events")]
    [SerializeField] private InputSelectEvent inpSelectEv;
    [SerializeField] private InputSelectPosEvent inpSelectPosEv;
    [SerializeField] private InputCamMoveEvent inpCamMoveEv;
    [SerializeField] private InputCamRotateEvent inpCamRotateEv;

    private void Awake()
    {
        inputAM = new InpActionMap(); 
    }

    private void OnEnable()
    {
        inputAM.InGameAP.Enable();
        inputAM.CameraAM.Enable();

        inputAM.InGameAP.select.performed += OnSelect;
        inputAM.InGameAP.selectPos.performed += OnSelectPos;
    }

    private void OnDisable()
    {
        inputAM.InGameAP.select.performed -= OnSelect;
        inputAM.InGameAP.selectPos.performed -= OnSelectPos;
        inputAM.InGameAP.Disable();
        inputAM.CameraAM.Disable();
    }
    public void OnSelect(InputAction.CallbackContext context)
    {
        bool inputAct = context.ReadValueAsButton();
        inpSelectEv.Invoke(inputAct);
        Debug.Log(inputAct);

    }
    public void OnSelectPos(InputAction.CallbackContext context)
    {
        Vector2 inputPos = context.ReadValue<Vector2>();
        inpSelectPosEv.Invoke(inputPos.x, inputPos.y);
        Debug.Log(inputPos);
    }
    */
}
