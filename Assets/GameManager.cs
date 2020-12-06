using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpawnState
{
    SPAWNING,
    WAITING,
    COUNTING,
}

public class GameManager : MonoBehaviour
{
    public SpawnState state;
    public float timeBetweenWaves = 5f;
    public float countDown = 2f;
    public Transform spawnPoint;

    private List<Transform> enemies = new List<Transform>();
    private int waveIndex = 0;

    private void Start()
    {
        StartCoroutine(RunSpawner());
    }

    private IEnumerator RunSpawner()
    {
        yield return new WaitForSeconds(countDown);

        while(true)
        {
            state = SpawnState.SPAWNING;
            yield return SpawnWave();

            state = SpawnState.WAITING;
            yield return new WaitWhile(EnemyisAlive);

            state = SpawnState.COUNTING;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    bool EnemyisAlive()
    {
        enemies = enemies.Where(e => e != gameObject.activeSelf == false).ToList();

        return enemies.Count > 8;
    }

    IEnumerator SpawnWave()
    {
        Debug.Log("Spawning Wave");
        waveIndex++;
        for(int i= 0; i<waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Debug.Log("Spawning enemy");
        GameObject enemy = GameObjectPool.SharedInstance.GetPooledObject("Enemy");
        if (enemy != null)
        {
            enemy.SetActive(true);
            enemy.transform.position = spawnPoint.position;
            enemies.Add(enemy.transform);
        }
    }
}
