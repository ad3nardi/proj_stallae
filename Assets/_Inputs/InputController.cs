using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>
{
    [Header("Plugins")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private player_manager _pm;

    [Header("Input Actions")]
    [SerializeField] private InputAction inpSelect;
    [SerializeField] private InputAction inpSelectPos;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _pm = GetComponent<player_manager>();
    }

    private void OnEnable()
    {
        _playerInput.currentActionMap.Enable();
        
        //Attatch Input Actions
        inpSelect = _playerInput.actions["select"];
        inpSelectPos = _playerInput.actions["selectPos"];
        
        //Subscribe to Input Events
        inpSelect.performed += select;
        inpSelect.canceled += selectCanceled;
        inpSelectPos.performed += selectPos;
    }
    
    private void OnDisable()
    {
        //Unsubscribe from Input Events
        inpSelect.performed -= select;
        inpSelect.canceled -= selectCanceled;
        inpSelectPos.performed -= selectPos;
    }

    private void select(InputAction.CallbackContext context)
    {
        bool inpAct = context.ReadValueAsButton();
        _pm.InputSelect(inpAct);
    }

    private void selectPos(InputAction.CallbackContext context)
    {
        Vector2 inpAct = context.ReadValue<Vector2>();
        _pm.InputSelectPos(inpAct.x, inpAct.y);
    }

    private void selectCanceled(InputAction.CallbackContext context)
    {
        bool inpAct = context.ReadValueAsButton();
        _pm.InputRelease(inpAct);
    }
}
