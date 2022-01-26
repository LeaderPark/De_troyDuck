using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;
    public Transform mainSpawn;
    public void EnemySpawn()
    {
        StartCoroutine(spawn());
        StartCoroutine(MainSpawn());
    }

    IEnumerator spawn()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(enemy, spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator MainSpawn()
    {
        while(true)
        {
            Instantiate(enemy, mainSpawn.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}
