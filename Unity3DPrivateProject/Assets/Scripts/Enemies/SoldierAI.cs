using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    public GameObject soldier;
    private Animator animator;


    private void Awake()
    {
        animator = soldier.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.Play("assault_combat_shoot");
        soldier.GetComponent<NavigationAI>().enabled = false;
        soldier.GetComponent<NavMeshAgent>().enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        animator.Play("assault_combat_run");
        soldier.GetComponent<NavigationAI>().enabled = true;
        soldier.GetComponent<NavMeshAgent>().enabled = true;
    }
}
