using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private bool rotate; // do you want it to rotate?
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float healthAmount;

    [SerializeField]
    private AudioClip collectSound;
    [SerializeField]
    private GameObject collectEffect;

    private void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(other.GetComponent<PlayerHealth>().currentHealth < other.GetComponent<PlayerHealth>().GetMaxHealth())
            {
                // add sound maybe effect
                if (collectSound)
                    AudioSource.PlayClipAtPoint(collectSound, transform.position);
                if (collectEffect)
                    Instantiate(collectEffect, transform.position, Quaternion.identity);

                other.GetComponent<PlayerHealth>().AddHealth(healthAmount);

                gameObject.SetActive(false);
            }

        }
    }
}
