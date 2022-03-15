using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 50f;
    [SerializeField]
    private float maxHealth =50f;

    [SerializeField]
    private GameObject healthBarUI;
    [SerializeField]
    private Slider healthSlider;

    private void Start()
    {
        health = maxHealth;
        healthSlider.value = CalculateHealth();
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


    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Hit player");
            collision.collider.GetComponent<PlayerHealth>().TakeDamage(10f);
        }
    }
}
