using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCon : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        touchPositionAction = playerInput.actions["select"];
        touchPositionAction = playerInput.actions["selectPos"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += Select;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= Select;

    }

    private void Select(InputAction.CallbackContext context)
    {
        Vector2 inputPos = touchPositionAction.ReadValue<Vector2>();
        Vector3 position = cam.ScreenToWorldPoint(inputPos);
            position.z = 0;

        float input = context.ReadValue<float>();

    }
}
