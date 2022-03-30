using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private AudioClip deathSound;

    private PauseMenu pm;
    public float currentHealth { get; private set; }

    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        
    }
    private void Start()
    {
        GetComponent<PlayerRagdoll>().RagdollSetActive(false);
        pm = GameObject.FindGameObjectWithTag("PM").GetComponent<PauseMenu>();
    }
    public float GetMaxHealth()
    {
        return startingHealth;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            AudioSource.PlayClipAtPoint(hurtSound, gameObject.transform.position);
        }
        else
        {
            if (!dead)
            {
                dead = true;
                Debug.Log("You died");
                Die();
                AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
            }
        }

    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private void Die()
    {
        //Activate Ragdoll Mode
        GetComponent<PlayerRagdoll>().RagdollSetActive(true);

        pm.GameOver();
    }
}
