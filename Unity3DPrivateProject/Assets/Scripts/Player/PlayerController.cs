using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header ("Player Specs")]
    [SerializeField, Tooltip("How fast player moves")]
    private float playerSpeed = 2.0f;
    [SerializeField, Tooltip("How high player jumps")]
    private float jumpHeight = 1.0f;
    [SerializeField, Tooltip("How much value does the downwards force have")]
    private float gravityValue = -9.81f;
    [SerializeField, Tooltip("How fast player rotates")]
    private float rotationSpeed = 10f;



    [Header("Animations speed")]
    [SerializeField, Tooltip("Value used for smoothing animation")]
    private float animationSmoothTime = 0.1f;
    [SerializeField, Tooltip("Value used animation transition")]
    private float animationPlayTransition = 0.15f;

    [Header("Aim Specs")]
    [SerializeField, Tooltip("Transform for where you aim")]
    private Transform aimTarget;
    [SerializeField, Tooltip("Value for the distance of the aim")]
    private float aimDistance = 1f;


    [SerializeField]
    private AudioClip jumpSound;


    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Transform cameraTransform;

    //Cached player input action to avoid using string refrence many times such as "Move"
    private InputAction moveAction;
    private InputAction jumpAction;



    private Animator animator;
    int jumpAnimation;
   

    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;


    private void Awake()
    {
        
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        // Cache a reference to all of the input actions
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        
        // Lock the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //Animations
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Rifle Jump");
      
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        animator.SetFloat(moveXAnimationParameterId, 1f);
    }

    private void Aim()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
    }
    void Update()
    {
        

        Aim();
        groundedPlayer = controller.isGrounded;
        // If the player is on the ground, don't aplly a downwards force
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector,input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        // Take into account the camera direction when moving the player
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (groundedPlayer == true && controller.velocity.magnitude > 2f && GetComponent<AudioSource>().isPlaying == false)
        {
            GetComponent<AudioSource>().volume = Random.Range(0.8f, 1f);
            GetComponent<AudioSource>().volume = Random.Range(0.8f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
            
        
        // Blend Strafe Animation
        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);
        // Changes the height position of the player.
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            AudioSource.PlayClipAtPoint(jumpSound, gameObject.transform.position);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate the player towards aim/camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
