using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbodyParent;
    private Collider colliderParent;
    private PlayerController playerController;
    private Gun gun;

    [SerializeField]
    private GameObject weapon;

    private Collider[] childrenCollider;
    private Rigidbody[] childrenRigidbody;

     void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbodyParent = GetComponent<Rigidbody>();
        colliderParent = GetComponent<Collider>();
        playerController = GetComponent<PlayerController>();
        gun = GetComponent<Gun>();

        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollSetActive(bool activate)
    {
        if (animator == null)
            Awake();
        //children
        foreach (var collider in childrenCollider)
            collider.enabled = activate;
        foreach (var rigidbody in childrenRigidbody)
        {
            rigidbody.isKinematic = !activate;
            rigidbody.detectCollisions = activate;
        }

        weapon.SetActive(!activate);
        //parent
        colliderParent.enabled = !activate;
        rigidbodyParent.isKinematic = activate;
        rigidbodyParent.detectCollisions = !activate;
        animator.enabled = !activate;
        playerController.enabled = !activate;
        gun.enabled = !activate;
    }
}
