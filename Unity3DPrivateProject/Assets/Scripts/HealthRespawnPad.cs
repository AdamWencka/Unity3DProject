using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRespawnPad : MonoBehaviour
{
    [SerializeField]
    private GameObject healthPack;

    public float timeToRespawn;


    private void Start()
    {
        StartCoroutine(RespawnHealthPack());
    }




    IEnumerator RespawnHealthPack()
    {
        while (true)
        {
            if (healthPack.activeInHierarchy)
                yield return null;
            if (!healthPack.activeInHierarchy)
            {
                yield return new WaitForSeconds(timeToRespawn);
                healthPack.SetActive(true);
            }

        }
        
    }
}
