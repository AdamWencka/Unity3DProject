using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public float health = 50f;

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }
}
