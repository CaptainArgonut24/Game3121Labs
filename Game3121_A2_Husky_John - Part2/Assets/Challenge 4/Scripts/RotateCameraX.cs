using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200f;
    public GameObject player;
    public PlayerInputActions PlayerInputActions;
    public InputAction Look;
    public InputAction LookB;

    private float rotationInput = 0f;
    private bool isReversing = false;

    void Awake()
    {
        // Initialize input actions
        PlayerInputActions = new PlayerInputActions();
        Look = PlayerInputActions.Player.Look;  // For 'A'
        LookB = PlayerInputActions.Player.LookB; // For 'D'
    }

    void OnEnable()
    {
        PlayerInputActions.Enable();
        Look.performed += OnLookInput; // Subscribe to Look action
        Look.canceled += OnLookInput; // Reset when released
        LookB.performed += OnLookBInput; // Subscribe to LookB action
        LookB.canceled += OnLookBInput; // Reset when released
    }

    void OnDisable()
    {
        Look.performed -= OnLookInput; // Unsubscribe to avoid memory leaks
        Look.canceled -= OnLookInput;
        LookB.performed -= OnLookBInput;
        LookB.canceled -= OnLookBInput;
        PlayerInputActions.Disable();
    }

    void OnLookInput(InputAction.CallbackContext context)
    {
        // Update rotation input for 'A'
        rotationInput = context.ReadValue<float>();
        isReversing = false; // Reset reversing state
    }

    void OnLookBInput(InputAction.CallbackContext context)
    {
        // Set reverse direction for 'D'
        isReversing = context.ReadValue<float>() > 0;
    }

    void Update()
    {
        // Calculate rotation amount
        float rotationAmount = rotationInput * speed * Time.deltaTime;

        if (isReversing)
        {
            rotationAmount = -speed * Time.deltaTime; // Reverse direction for LookB
        }

        // Apply rotation around the Y-axis (0, 1, 0)
        transform.Rotate(new float3(0f, 1f, 0f), rotationAmount);

        // Keep camera at the player's position
        transform.position = player.transform.position;
    }
}
