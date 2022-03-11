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

    [Header("Prefabs and usefull transforms")]
    [SerializeField, Tooltip("Bullet Prefab")]
    private GameObject bulletPrefab;
    [SerializeField, Tooltip("Place where bullets spawn")]
    private Transform barrelTip;
    [SerializeField, Tooltip("Where the spawned bullet can be held")]
    private Transform bulletParent;
    [SerializeField, Tooltip("How far can the bullet travel if it doesn't hit anything")]
    private float bulletHitMissDistance = 25f;

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

    [Header("Fire Rate")]
    [SerializeField, Tooltip("Fire rate of the rifle")]
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;


    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Transform cameraTransform;

    //Cached player input action to avoid using string refrence many times such as "Move"
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;


    private Animator animator;
    int jumpAnimation;
    int recoilAnimation;

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
        shootAction = playerInput.actions["Shoot"];
        // Lock the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //Animations
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Rifle Jump");
        recoilAnimation = Animator.StringToHash("Rifle Shoot Recoil");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        animator.SetFloat(moveXAnimationParameterId, 1f);
    }
     // The commented code is great for single fire rifle or for shooting as fast as the button is pressed (LMB)
    /*private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
        shootAction.started += _ => ShootGun();
        
    }
    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
        shootAction.started -= _ => ShootGun();

    }*/
        /// <summary>
        /// Spawn a bullet and shoot in the direction of the gun barrel. If the raycast hits the enviroment
        /// bullet travels towards the point of contact, else if it will not have a target and it will be destroyed
        /// at a certain distance away from the player
        /// </summary>
    private void ShootGun()
    {
         RaycastHit hit;
         GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTip.position, Quaternion.identity, bulletParent);
         BulletController bulletController = bullet.GetComponent<BulletController>();
         if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
          {
             Vector3 aimdir = (hit.point - barrelTip.position).normalized;
             bulletController.transform.rotation = Quaternion.LookRotation(aimdir, Vector3.up);
             bulletController.target = hit.point;
             bulletController.hit = true;
         }
         else
         {
             bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
             bulletController.hit = false;
         }

        animator.CrossFade(recoilAnimation, animationPlayTransition);
    }
    private void Aim()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;
    }
    void Update()
    {
        // Fire Gun while pressing mouse button using new input system and constraint that with rate of fire 
        if (shootAction.ReadValue<float>() > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            ShootGun();
        }

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
        // Blend Strafe Animation
        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);
        // Changes the height position of the player.
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate the player towards aim/camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
