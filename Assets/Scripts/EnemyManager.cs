using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Transform> spawnPoints = new List<Transform>();
    //public Transform spawnPoint;
    public GameObject enemy;

    public List<GameObject> enemies = new List<GameObject>();

    private float spawnRate = 2f;
    private float spawnRateChangeRate = 5f;
    private float numSpawnPointsChangeRate = 15f;
    private float nextSpawn = 0f;
    private float nextSpawnRateChangeRate;
    private float nextNumSpawnPointsChangeRate;
    private int spawnPointRange = 1;

    private void Start()
    {
        nextSpawnRateChangeRate = spawnRateChangeRate;
        nextNumSpawnPointsChangeRate = numSpawnPointsChangeRate;
    }

    void Update()
    {
        if (Time.time > nextSpawn && GameManager.Instance.GameOn)
        {
            nextSpawn = Time.time + spawnRate;
            Instantiate(enemy, spawnPoints[Random.Range(0,spawnPointRange)].position, Quaternion.identity);

            if (Time.time > nextSpawnRateChangeRate && spawnRate > 1f)
            {
                nextSpawnRateChangeRate = Time.time + spawnRateChangeRate;
                spawnRate -= 0.05f;
            }

            if (Time.time > nextNumSpawnPointsChangeRate && spawnPointRange <= spawnPoints.Count)
            {
                nextNumSpawnPointsChangeRate = Time.time + numSpawnPointsChangeRate;
                spawnPointRange += 1;
            }
        }
    }

    public GameObject GetClosestEnemy(Transform obj)
    {
        GameObject nearest = null;
        float smallestDistance = float.PositiveInfinity;
        if (enemies.Count < 1)
        {
            return null;
        }
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, obj.position) < smallestDistance)
            {
                smallestDistance = Vector3.Distance(enemy.transform.position, obj.position);
                nearest = enemy;
            }
        }
        return nearest;
    }
}
