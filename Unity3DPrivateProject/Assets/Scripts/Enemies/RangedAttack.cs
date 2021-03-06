using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public NavMeshAgent agent;
    [SerializeField]
    private float damage;
    public Bullet bulletPrefab;
    [SerializeField]
    private float attackRate = 1f;
    private float nextAttack = 0.0f;
    public Transform bulletSpawnOffset; // Add empty gameobject in front of barrel 
    private ObjectPool bulletPool;

    private Bullet bullet;
    public Enemy enemy;
    public EnemyMovement enemyMovement;
    [SerializeField]
    private float range;
    public Transform target;

    bool detected = false;

    [Header("Muzzle Flash")]
    [SerializeField, Tooltip("Muzzle Flash")]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField, Tooltip("Value used animation transition")]
    private float animationPlayTransition = 0.15f;
    int rifleShootAnimation;
    

    Vector3 direction;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackRate) * bulletPrefab.autoDestroyTime));
        rifleShootAnimation = Animator.StringToHash("assault_combat_shoot");
    }
    private void Update()
    {
        Vector3 targetPos = target.position;

        direction = targetPos - transform.position;
        RaycastHit rayInfo;

        if (Physics.Raycast(transform.position + Vector3.up, direction, out rayInfo, range))
        {
            if (rayInfo.collider.tag == "Player")
            {
                    detected = true;
                    agent.enabled = false;
                    ShootPlayer();
            }
            else
            {
                agent.enabled = true;
                detected = false;
            }
            if(Vector3.Distance(transform.position, rayInfo.collider.transform.position) > range)
            {
                agent.enabled = true;
                detected = false;
            }
        }
    }

    private void FixedUpdate()
    {

        if (detected)
        {
            enemy.target = target.transform;
            enemy.OnAttack();

        }

    }

    void ShootPlayer()
    {
        if (Time.time > nextAttack)
        {
            muzzleFlash.Play();
            AudioSource.PlayClipAtPoint(shootSound, bulletSpawnOffset.position);
            PoolableObject poolableObject = bulletPool.GetObject();
            
            nextAttack = Time.time + 1f / attackRate;
            animator.CrossFade(rifleShootAnimation, animationPlayTransition);
            bullet = poolableObject.GetComponent<Bullet>();
            bullet.damage = damage;
            bullet.transform.position = bulletSpawnOffset.position;
            bullet.transform.rotation = agent.transform.rotation;
            bullet.Rigidbody.AddForce(agent.transform.forward * bulletPrefab.moveSpeed, ForceMode.VelocityChange);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position + Vector3.up, direction);
        Gizmos.DrawWireSphere(transform.position, range);
    }






}
