using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public GameObject Prefab;
    public int Cost;
    public int Bias;
}
public class EnemySpawner : MonoBehaviour
{
    public EnemySpawn[] EnemyPrefabs;
    private GameObject Player;

    public float WaveDelay = 10;
    private float CurrentWaveDelay = 0;
    private int EnemyResourse = 5;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CurrentWaveDelay -= Time.deltaTime;

        if(CurrentWaveDelay <= 0)
        {
            Vector3 spawnPosition = Vector3.zero;
            int resourseRemaining = EnemyResourse;
            while(resourseRemaining > 0)
            {
                int enemyToSpawn = 0;

                // bias to spawn any given enemy
                int TotalBias = 0;
                foreach (EnemySpawn e in EnemyPrefabs)
                {
                    if (e.Cost <= resourseRemaining)
                        TotalBias += e.Bias;
                }
                int bias = Random.Range(0, TotalBias);

                for (int i = 0; i < EnemyPrefabs.Length; i++)
                {
                    if (EnemyPrefabs[i].Cost <= resourseRemaining)
                    {
                        bias -= EnemyPrefabs[i].Bias;
                        if (bias <= 0)
                        {
                            enemyToSpawn = i;
                            break;
                        }
                    }
                }

                spawnPosition = Player.transform.position;
                if (Random.Range(0,2) == 0)
                {
                    spawnPosition.x += Random.Range(0, 2) == 0 ? 40 : -40;
                    spawnPosition.z += Random.Range(-40, 40);
                }
                else
                {
                    spawnPosition.z += Random.Range(0, 2) == 0 ? 40 : -40;
                    spawnPosition.x += Random.Range(-40, 40);
                }
                Instantiate(EnemyPrefabs[enemyToSpawn].Prefab, spawnPosition, Quaternion.identity);
                resourseRemaining -= EnemyPrefabs[enemyToSpawn].Cost;
            }
            CurrentWaveDelay += WaveDelay;
            EnemyResourse += 1;
        }
    }
}
