using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    [SerializeField] private Gun playerGun;
    [SerializeField] private Text ammoCount;
    private void Start()
    {
        ammoCount.text = playerGun.currentAmmo.ToString();
    }
    private void Update()
    {
        ammoCount.text = playerGun.currentAmmo.ToString();
    }

}
