using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject enemyPrefab;
    public float spawnTime;
    float spawnTimeCounter;
    public Transform enemyParent;

    Vector2 screenHalfSizeWorldsUnits;

    private void Start()
    {
        spawnTimeCounter = spawnTime;
        screenHalfSizeWorldsUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    private void Update()
    {
        if (GameManager.Instance.IsEnded()) return;

        if(spawnTimeCounter < 0)
        {
            SpawnEnemy();
            spawnTimeCounter = spawnTime;
        }
        spawnTimeCounter -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = new Vector2(
                    Random.Range(-screenHalfSizeWorldsUnits.x + 4, screenHalfSizeWorldsUnits.x - 4),
                    screenHalfSizeWorldsUnits.y
                );
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition + (Vector2)transform.position, Quaternion.identity);
        enemy.transform.SetParent(enemyParent);
    }

}
