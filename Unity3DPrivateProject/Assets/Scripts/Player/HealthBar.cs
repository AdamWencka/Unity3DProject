using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    [SerializeField] private Text healthAmmount;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 100;
        healthAmmount.text = playerHealth.currentHealth.ToString();
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 100;
        healthAmmount.text = playerHealth.currentHealth.ToString();
    }
}
