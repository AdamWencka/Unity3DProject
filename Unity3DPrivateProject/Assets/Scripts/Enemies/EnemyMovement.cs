using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    
    public Transform target;
    [SerializeField]
    private Animator animator;
    public float updateSpeed = 0.1f; // How frequently to recalculate path based on target transfom position

    private NavMeshAgent agent;

    private const string IsWalking = "isWalking";

    private Coroutine followCoroutine;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void StartChasing()
    {
        if (followCoroutine == null)
            StartCoroutine(FollowTarget());
        else
            Debug.LogWarning("Called StartCoroutine on Enemy is already chasing! This is likely a bug in some calling class!");
    }

    private void Update()
    {
        animator.SetBool(IsWalking, agent.velocity.magnitude > 0.01f);
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);
        while (true)
        {
            if (agent.enabled)
            {
                agent.SetDestination(target.transform.position);
            }
   
  
            if(agent.enabled == true && GetComponent<AudioSource>().isPlaying == false)
            {  
                GetComponent<AudioSource>().volume = Random.Range(0.8f,1f);
                GetComponent<AudioSource>().volume = Random.Range(0.8f, 1.1f);
                GetComponent<AudioSource>().Play();
            }
            
               

            yield return wait;
        }
    }
}
