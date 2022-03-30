using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];

    }
    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }
    
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }


    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount;
        if (thirdPersonCanvas != null && aimCanvas != null)
        {
            aimCanvas.enabled = true;
            thirdPersonCanvas.enabled = false;
        }
  
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        if (thirdPersonCanvas != null && aimCanvas != null)
        {
            aimCanvas.enabled = false;
            thirdPersonCanvas.enabled = true;
        }

    }
}
