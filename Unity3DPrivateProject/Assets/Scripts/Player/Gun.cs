using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Parameters")]
    [SerializeField, Tooltip("Damage of the gun")]
    private float damage = 10f;
    [SerializeField, Tooltip("Range of the gun")]
    private float range = 100f;
    [SerializeField, Tooltip("Impact force of the gun")]
    private float impactForce = 30f;

    [SerializeField, Tooltip("Max Ammo of the gun")]
    private int maxAmmo = 15;
    public int currentAmmo { get; private set; }
    [SerializeField, Tooltip("Reload Time of the gun")]
    private float reloadTime= 5f;
    private bool isReloading;

    [Header("Third Person Camera")]
    [SerializeField, Tooltip("Third Person Camera")]
    private Camera tpsCam;

    [Header("Impact Effect")]
    [SerializeField, Tooltip("Concrete Impact Effect")]
    private GameObject concreteImpactEffect;
    [SerializeField, Tooltip("Blood Impact Effect")]
    private GameObject bloodImpactEffect;

    [Header("Muzzle Flash")]
    [SerializeField, Tooltip("Muzzle Flash")]
    private ParticleSystem muzzleFlash;

    [Header("Fire Rate")]
    [SerializeField, Tooltip("Fire rate of the rifle")]
    private float fireRate = 10f;
    private float nextFire = 0.0f;


    [SerializeField, Tooltip("Value used animation transition")]
    private float animationPlayTransition = 0.15f;

    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputAction reloadAction;
    private Animator animator;
    int recoilAnimation;
    public LayerMask ignoreLayerMask;
    

    public UnityEvent shake;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
        reloadAction = playerInput.actions["Reload"];
        recoilAnimation = Animator.StringToHash("Rifle Shoot Recoil");

    }
    private void Start()
    {
        currentAmmo = maxAmmo;
    }
    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;
        if(reloadAction.triggered && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }
        if (currentAmmo <= 0)
        {
           StartCoroutine(Reload());
            return;
        }
        // Fire Gun while pressing mouse button using new input system and constraint that with rate of fire 
        if (shootAction.ReadValue<float>() > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + 1f/fireRate;
            Shoot();
            shake.Invoke();
        }


    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        // animate reloading
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
    // Hitscan
    void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(tpsCam.transform.position, tpsCam.transform.forward, out hit, range,~ignoreLayerMask))
        {
            
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Instantiate(bloodImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            if (hit.transform.tag != "Enemy")
                Instantiate(concreteImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            
        }
        animator.CrossFade(recoilAnimation, animationPlayTransition);
    }




    // For a bullet spawning
    /*[Header("Prefabs and usefull transforms")]
    [SerializeField, Tooltip("Bullet Prefab")]
    private GameObject bulletPrefab;
    [SerializeField, Tooltip("Place where bullets spawn")]
    private Transform barrelTip;
    [SerializeField, Tooltip("Where the spawned bullet can be held")]
    private Transform bulletParent;
    [SerializeField, Tooltip("How far can the bullet travel if it doesn't hit anything")]
    private float bulletHitMissDistance = 25f;*/

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
    /* private void ShootGun()
     {
          RaycastHit hit;
          GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTip.position, Quaternion.identity, bulletParent);
          BulletController bulletController = bullet.GetComponent<BulletController>();
         Transform hitTransform = null;
          if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
           {
             Vector3 aimdir = (hit.point - barrelTip.position).normalized;
             bulletController.transform.rotation = Quaternion.LookRotation(aimdir, Vector3.up);
             bulletController.target = hit.point;
             bulletController.hit = true;
             hitTransform = hit.transform;
             if(hitTransform != null)
             {

             }
          }
          else
          {
              bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
              bulletController.hit = false;
          }

         animator.CrossFade(recoilAnimation, animationPlayTransition);
     }*/
}
