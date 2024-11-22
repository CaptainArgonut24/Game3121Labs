using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    
    public PlayerInputActions playerInputAction;
    private InputAction playerMovement;

   
    public float boundaryYUpper = 14.0f;
    public float boundaryYLower = 1.0f;

    
    public float bounceForce;
    public AudioClip bounceSound;

    private void Awake()
    {
        //Q 1. setting them up
        playerInputAction = new PlayerInputActions();
        
    }
    
    private void OnEnable()
    {
        Debug.Log("Input Actions Enabled");
        playerMovement = playerInputAction.Player.FloatUp;
        playerInputAction.Enable();
        playerMovement.performed += FloatUp;
    }

    private void OnDisable()
    {
        Debug.Log("Input Actions Disabled");
        playerInputAction.Disable();
        playerMovement.performed -= FloatUp;
    }


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        
        playerRb.AddForce(new float3(0, 5, 0), ForceMode.Impulse);
    }


    // Update is called once per frame
    void Update()
    {   
        
        BoundaryCheck();

       
        if(transform.position.y <= boundaryYLower)
        {
            Bounce();
        }
    }

    public void FloatUp(InputAction.CallbackContext context)
    {
        

        if (!gameOver)
        {
            playerRb.AddForce(new float3(0, floatForce, 0), ForceMode.Impulse);  // Use float3 for force direction
        }
    }
    // JUST BLOW UP   
    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");

            Destroy(other.gameObject); // Destroy the bomb immediately

            // Start a coroutine to destroy the player after effects
            StartCoroutine(DestroyPlayerAfterEffects());
        }
        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }

    // Coroutine to destroy the player object after a short delay
    private IEnumerator DestroyPlayerAfterEffects()
    {
        yield return new WaitForSeconds(1.5f); // Adjust the delay as needed
        Destroy(gameObject); // Remove the player object
    }



    public void BoundaryCheck()
    {
        
        float3 currentPosition = new float3(transform.position.x, transform.position.y, transform.position.z);
        
        currentPosition.y = math.clamp(currentPosition.y, boundaryYLower, boundaryYUpper);
       
        transform.position = new float3(currentPosition.x, currentPosition.y, currentPosition.z);
    }

   
    public void Bounce()
    {
        //reset vert vel
        playerRb.velocity = new float3(playerRb.position.x, 0, playerRb.velocity.z);
        // playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        playerRb.AddForce(new float3(0, bounceForce, 0), ForceMode.Impulse);
        playerAudio.PlayOneShot(bounceSound, 1.0f);
    }

}


