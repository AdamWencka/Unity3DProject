using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    public float health = 50f;
    private float maxHealth =50f;
    public delegate void DeathEvent(Enemy enemy);
    public DeathEvent OnDie;
    [SerializeField]
    private GameObject healthBarUI;
    [SerializeField]
    private Slider healthSlider;

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
        gameObject.SetActive(false);
        
    }

    public Transform GetTransform()
    {
        return transform;
    }


}
