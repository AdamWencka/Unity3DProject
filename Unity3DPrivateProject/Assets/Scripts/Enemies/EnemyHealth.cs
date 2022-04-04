using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    
    public float health = 50f;
    private float maxHealth =50f;
    public delegate void DeathEvent(Enemy enemy);
    public DeathEvent OnDie;
    [SerializeField]
    private GameObject healthBarUI;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private GameObject deathEffect;


    private void Start()
    {
        health = maxHealth;
        healthSlider.value = CalculateHealth();
    }
    private void OnEnable()
    {
        health = maxHealth;
        healthSlider.value = CalculateHealth();
        healthBarUI.SetActive(false);
    }
    private void Update()
    {
        healthSlider.value = CalculateHealth();

        if(health< maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void TakeDamage(float amount)
    {
        AudioSource.PlayClipAtPoint(hurtSound, gameObject.transform.position);
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }

    private void OnDisable()
    {
        OnDie = null;
    }

    void Die()
    {
        OnDie?.Invoke(GetComponent<Enemy>());
        ScoreCount.instance.IncrementScore();
        Instantiate(deathEffect, transform.position + transform.up, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);
        gameObject.SetActive(false);
        
    }

    public Transform GetTransform()
    {
        return transform;
    }


}
