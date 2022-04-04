using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private int numberOfEnemiesToSpawn = 5;
    [SerializeField]
    private float spawnDelay = 1f;
    [SerializeField]
    private List<Enemy> enemyPrefabs = new List<Enemy>();
    [SerializeField]
    private SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    [SerializeField]
    private int level = 0;

    private int enemiesAlive = 0;
    private int spawnedEnemies = 0;
    [SerializeField]
    private bool continousSpawning;

    private NavMeshTriangulation triangulation;
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(enemyPrefabs[i], numberOfEnemiesToSpawn));
        }
    }


    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        level++;
        spawnedEnemies = 0;
        enemiesAlive = 0;
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);


        while(spawnedEnemies < numberOfEnemiesToSpawn)
        {
            if(enemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(spawnedEnemies);
            }
            else if(enemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            spawnedEnemies++;
            yield return wait;
        }
        if (continousSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int spawnIndex = spawnedEnemies % enemyPrefabs.Count;

        DoSpawnEnemy(spawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemyPrefabs.Count));
    }

    public void DoSpawnEnemy(int SpawnIndex)
    {
        PoolableObject poolableObject = enemyObjectPools[SpawnIndex].GetObject();

        if(poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

          
            int vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if(NavMesh.SamplePosition(triangulation.vertices[vertexIndex],out hit, 2f, -1))
            {
                enemy.agent.Warp(hit.position);
                // enemy needs to get enabled and start chasing now.
                enemy.movement.target = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChasing();
                enemy.GetComponent<EnemyHealth>().OnDie += HandleEnemyDeath;
                enemiesAlive++;
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {triangulation.vertices[vertexIndex]}");
            }

        }
        else
        {
            Debug.LogError($"Unable to fetxh enemy of type {SpawnIndex} from object pool. Out of objects?");
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemiesAlive--;
        if(enemiesAlive == 0 && spawnedEnemies == numberOfEnemiesToSpawn)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random
    }
}
