using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyReference;

    [SerializeField] private Transform gatePosition;

    [SerializeField] private float minEnemyScaleAddition = 0.9f;

    [SerializeField] private float maxEnemyScaleAddition = 13.0f;

    [SerializeField] private float enemyScaleBias = 7.0f;

    [HideInInspector] public int enemiesOnMap = 0;

    private int randomIndex;

    private GameObject spawnedEnemy;

    private GameObject playerObject;

    private void Awake()
    {
    }
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        Debug.Log(playerObject);

        if (playerObject != null) {
            while (enemiesOnMap < 999 && NewPlayer.instance.playerDead == false)
            {
                randomIndex = Random.Range(0, enemyReference.Length);

                spawnedEnemy = Instantiate(enemyReference[randomIndex]);
                enemiesOnMap++;

                spawnedEnemy.transform.position = gatePosition.position;

                Debug.Log("Reached floats");
                float randomPlus = Random.Range(0.7f, 1.6f);
                float randomMinus = Random.Range(-0.2f, -0.8f);
                Debug.Log(CoinFlip());
                switch (CoinFlip())
                {
                    case "Sideways":
                        spawnedEnemy.transform.localScale = playerObject.transform.localScale + new Vector3(3f, 3f, 0f);
                        break;
                    case "Heads":
                        spawnedEnemy.transform.localScale = playerObject.transform.localScale + new Vector3(randomPlus, randomPlus, 0f);
                        break;
                    case "Tails":
                        spawnedEnemy.transform.localScale = playerObject.transform.localScale + new Vector3(randomMinus, randomMinus, 0f);
                        break;
                    default:
                        Debug.Log("Something went wrong!");
                        break;
                }

                yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
            }
        }
    }

    private string CoinFlip()
    {
        float random = Random.value;
        if (random > 0.98f) return "Sideways";
        else if (random > 0.49f) return "Heads";
        else return "Tails";
    }

}//class
