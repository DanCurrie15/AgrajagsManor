using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform spawnPoint;
    public GameObject enemy;

    private float spawnRate = 2f;
    private float nextSpawn = 0f;

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
    }
}
