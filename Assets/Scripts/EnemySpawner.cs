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

    private GameObject spawnedEnemy;


    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }


    void Update()
    {
        
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
        }
    }

}//class
