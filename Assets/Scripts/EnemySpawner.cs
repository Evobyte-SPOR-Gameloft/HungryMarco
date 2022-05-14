using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyReference;

    [SerializeField]
    private Transform gatePosition;

    [HideInInspector]
    public int enemiesOnMap = 0;

    private int randomIndex;
    private float enemyRandomScaleAddition;

    private GameObject spawnedEnemy;

    [SerializeField]
    private float minEnemyScaleAddition = 0.5f;

    [SerializeField]
    private float maxEnemyScaleAddition = 7.0f;

    [SerializeField]
    private float enemyScaleBias = 10.0f;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesOnMap < 999)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));

            randomIndex = Random.Range(0, enemyReference.Length);

            spawnedEnemy = Instantiate(enemyReference[randomIndex]);
            enemiesOnMap++;

            spawnedEnemy.transform.position = gatePosition.position;

            enemyRandomScaleAddition = BiasedRandom(minEnemyScaleAddition, maxEnemyScaleAddition, enemyScaleBias);

            spawnedEnemy.transform.localScale += new Vector3(enemyRandomScaleAddition, enemyRandomScaleAddition, 0f);
        }
    }

    //bias < 1 => biased towards higher values
    //bias > 1 => biased towards lower values
    float BiasedRandom(float low, float high, float bias)
    {
        float x = Random.Range(0.1f, 0.9f);

        x = Mathf.Pow(x, bias);

        return low + (high - low) * x;
    }

}//class
