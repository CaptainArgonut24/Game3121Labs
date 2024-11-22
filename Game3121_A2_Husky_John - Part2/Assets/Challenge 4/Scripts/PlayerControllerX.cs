using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public PlayerInputActions PlayerInputActions;
    private InputAction move;
    private InputAction moveB;
    public InputAction Boost;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private float BoostStrength = 10;

    public GameObject BoostEffect;
    public ParticleSystem BoostParticles;

    private float moveInput = 0f; // For forward movement
    private bool isMovingBackward = false; // Track backward movement

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Awake()
    {
        PlayerInputActions = new PlayerInputActions();

        Boost = PlayerInputActions.Player.Boost;
        move = PlayerInputActions.Player.Move;
        moveB = PlayerInputActions.Player.MoveB;

        BoostEffect = GameObject.Find("Smoke_Particle");
        if (BoostEffect != null)
            BoostParticles = BoostEffect.GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        PlayerInputActions.Enable();
        move.performed += OnMoveInput;
        move.canceled += OnMoveInput;
        moveB.performed += OnMoveBackwardInput;
        moveB.canceled += OnMoveBackwardInput;
    }

    void OnDisable()
    {
        move.performed -= OnMoveInput;
        move.canceled -= OnMoveInput;
        moveB.performed -= OnMoveBackwardInput;
        moveB.canceled -= OnMoveBackwardInput;
        PlayerInputActions.Disable();
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        // Forward movement logic
        moveInput = context.ReadValue<float>();
    }

    void OnMoveBackwardInput(InputAction.CallbackContext context)
    {
        // Track whether MoveB is being pressed
        isMovingBackward = context.ReadValue<float>() > 0;
    }

    void BoostPowerup()
    {
        // Apply boost in the forward direction
        float3 forward = math.normalize(new float3(focalPoint.transform.forward.x, 0, focalPoint.transform.forward.z));
        playerRb.AddForce(forward * BoostStrength, ForceMode.Force);
    }

    void Update()
    {
        // Update powerup indicator position
        powerupIndicator.transform.position = (float3)transform.position + new float3(0, -0.6f, 0);

        // Boost logic
        if (Boost.IsPressed())
        {
            BoostPowerup();
            BoostEffect.transform.position = transform.position;
            BoostParticles.Play();
        }
    }

    void FixedUpdate()
    {
        // Calculate forward movement direction relative to the focal point
        float3 forward = math.normalize(new float3(focalPoint.transform.forward.x, 0, focalPoint.transform.forward.z));

        // Determine the force direction based on forward or backward input
        float3 forceDirection = forward * moveInput * speed * Time.fixedDeltaTime;

        if (isMovingBackward)
        {
            forceDirection = -forward * speed * Time.fixedDeltaTime; // Reverse direction for MoveB
        }

        // Apply the calculated force to the player Rigidbody
        playerRb.AddForce(forceDirection, ForceMode.Force);
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            float3 awayFromPlayer = math.normalize((float3)(other.gameObject.transform.position - transform.position));

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
