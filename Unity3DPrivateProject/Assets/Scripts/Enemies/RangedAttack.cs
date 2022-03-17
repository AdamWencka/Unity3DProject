using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : MonoBehaviour
{
    public Animator animator;
    public SphereCollider sphereCollider;
    public NavMeshAgent agent;
    public float damage;
    public Bullet bulletPrefab;
    public float attackRate = 1f;
    private float nextAttack = 0.0f;
    public Transform bulletSpawnOffset; // Add empty gameobject in front of barrel 
    public LayerMask layerMask;
    private ObjectPool bulletPool;
    [SerializeField]
    private float sphereCastRadius = 0.1f;
    private RaycastHit hit;
    private Bullet bullet;
    public Enemy enemy;
    public Transform target;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackRate) * bulletPrefab.autoDestroyTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && HasLineOfSightTo(other.transform))
        {
            enemy.target = other.transform;
            enemy.OnAttack();
            
            if (Time.time > nextAttack)
            {
                PoolableObject poolableObject = bulletPool.GetObject();
                agent.enabled = false;
                nextAttack = Time.time + 1f / attackRate;
                animator.SetTrigger("Shoot");
                bullet = poolableObject.GetComponent<Bullet>();
                bullet.damage = damage;
                bullet.transform.position = bulletSpawnOffset.position;
                bullet.transform.rotation = agent.transform.rotation;
                bullet.Rigidbody.AddForce(agent.transform.forward * bulletPrefab.moveSpeed, ForceMode.VelocityChange);

            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && HasLineOfSightTo(other.transform))
        {
            enemy.target = other.transform;
            enemy.OnAttack();
            if (Time.time > nextAttack)
            {
                PoolableObject poolableObject = bulletPool.GetObject();
                agent.enabled = false;
                nextAttack = Time.time + 1f / attackRate;
                animator.SetTrigger("Shoot");
                bullet = poolableObject.GetComponent<Bullet>();
                bullet.damage = damage;
                bullet.transform.position = bulletSpawnOffset.position;
                bullet.transform.rotation = agent.transform.rotation;
                bullet.Rigidbody.AddForce(agent.transform.forward * bulletPrefab.moveSpeed, ForceMode.VelocityChange);
               
            }

        }
    }

    private void Update()
    {
        HasLineOfSightTo(target);
    }


    private bool HasLineOfSightTo(Transform target)
    {
        if(Physics.SphereCast(bulletSpawnOffset.position, sphereCastRadius, ((target.position + bulletSpawnOffset.position) - (transform.position + bulletSpawnOffset.position)).normalized, out hit, sphereCollider.radius, layerMask))
        { 
            if(hit.collider.tag == "Player")
            {
                Debug.Log("Sees the player");
                return true;
            }
        }
        Debug.Log("Don't see the player");
        return false;
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            agent.enabled = true;
        }
    }

}
