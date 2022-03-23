using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRespawnPad : MonoBehaviour
{
    [SerializeField]
    private GameObject healthPack;

    public float timeToRespawn;



    
    void Update()
    {
        if (!healthPack.activeSelf)
            StartCoroutine(RespawnHealthPack());
    }


    IEnumerator RespawnHealthPack()
    {
        

        if (!healthPack.activeSelf)
        {
            yield return new WaitForSeconds(timeToRespawn);
            healthPack.SetActive(true);
        }
        yield return null;
    }
}
