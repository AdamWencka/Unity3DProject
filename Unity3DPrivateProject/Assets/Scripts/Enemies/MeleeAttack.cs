using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public float damage;
    public float attackRate = 1f;
    private float nextAttack = 0.0f;
   public Enemy enemy;
    [SerializeField]
    private AudioClip attackSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" )
        {
            enemy.target = other.transform;
            enemy.OnAttack();
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + 1f / attackRate;
                animator.SetTrigger("Attack");
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            enemy.target = other.transform;
            enemy.OnAttack();
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + 1f / attackRate;
                animator.SetTrigger("Attack");
                AudioSource.PlayClipAtPoint(attackSound, other.transform.position);
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

        }
    }


}
