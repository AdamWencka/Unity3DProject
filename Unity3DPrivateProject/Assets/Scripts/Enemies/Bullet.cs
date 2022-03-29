using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolableObject
{
    public float autoDestroyTime = 5f;
    public float moveSpeed = 10f;
    public float damage = 10f;
    public Rigidbody Rigidbody;
    [Header("Impact Effect")]
    [SerializeField, Tooltip("Enemy Bullet Impact Effect")]
    private GameObject enemyBulletImpactEffect;
   

    private const string DISABLE_METHOD_NAME = "Disable";

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, autoDestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth;
        if(other.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            playerHealth.TakeDamage(damage);
            
        }


        Disable();
    }
    private void Disable()
    {
        Instantiate(enemyBulletImpactEffect, transform.position, Quaternion.identity);
        CancelInvoke(DISABLE_METHOD_NAME);
        Rigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
