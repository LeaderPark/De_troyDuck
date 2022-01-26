using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;
    public void EnemySpawn()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(enemy, spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
