using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies
///     and to reset their stats if they died or were modified at runtime.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Configuration", menuName ="ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    //Enemy stats
    public float health = 100f;
    public float attackDelay = 1f;
    public float damage = 10f;
    public float attackRadius = 1.5f;
    public bool isRanged = false;


    //NavMeshAgent Configs
    public float AIUpdateInterval = 0.1f;

    public float acceleration = 8;
    public float angularSpeed = 120;
    // -1 means everything
    public int areaMask = -1;
    public int avoidancePriority = 50;
    public float baseOffset = 0;
    public float height = 2f;
    public ObstacleAvoidanceType obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float radius = 0.5f;
    public float speed = 3f;
    public float stoppingDistance = 0.5f;
}
