using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform spawnPoint;
    public GameObject enemy;

    public List<GameObject> enemies = new List<GameObject>();

    private float spawnRate = 2f;
    private float nextSpawn = 0f;

    void Update()
    {
        if (Time.time > nextSpawn && GameManager.Instance.GameOn)
        {
            nextSpawn = Time.time + spawnRate;
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
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
