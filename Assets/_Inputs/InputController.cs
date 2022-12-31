using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Plugins")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private player_manager _playerManager;
    [SerializeField] private camera_con _camManager;
    [Header("Input Actions")]
    [SerializeField] private InputAction inpSelect;
    [SerializeField] private InputAction inpSelectPos;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerManager = GetComponent<player_manager>();
        _camManager = GetComponent<camera_con>();
    }

    private void OnEnable()
    {
        _playerInput.currentActionMap.Enable();

        inpSelect = _playerInput.actions["select"];
        inpSelectPos = _playerInput.actions["selectPos"];

        inpSelect.performed += select;
        inpSelectPos.performed += selectPos;

    }
    
    private void OnDisable()
    {
        inpSelect.performed -= select;
        inpSelectPos.performed -= selectPos;
    }

    private void select(InputAction.CallbackContext context)
    {
        bool inpAct = context.ReadValueAsButton();
        _playerManager.InputSelect(inpAct);
    }

    private void selectPos(InputAction.CallbackContext context)
    {
        Vector2 inpAct = context.ReadValue<Vector2>();
        _playerManager.InputSelectPos(inpAct.x, inpAct.y);
    }
}
