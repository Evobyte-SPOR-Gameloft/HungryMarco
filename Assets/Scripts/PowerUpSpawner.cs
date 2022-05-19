using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpReference;

    private GameObject spawnedPowerUp;

    private GameObject referenceCollider;
    private BoxCollider2D allowedArea;

    private Vector2 target;

    private int randomIndex;
    private double chance;


    private void Awake()
    {
        referenceCollider = GameObject.FindWithTag("AllowedArea");

        if (referenceCollider != null)
        {
            allowedArea = referenceCollider.GetComponent<BoxCollider2D>();
        }
    }
    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            while (NewPlayer.instance.playerDead == false)
            {
                Debug.Log("Attempt to spawn PowerUp");

                chance = Random.value;

                Debug.Log(chance > 0.9);

                if (chance > 0.9)
                {
                    target = RandomPointInBounds(allowedArea.bounds);

                    randomIndex = Random.Range(0, powerUpReference.Length);

                    spawnedPowerUp = Instantiate(powerUpReference[randomIndex]);

                    spawnedPowerUp.transform.position = target;
                }

                yield return new WaitForSeconds(5);
            }
        }
    }

    private static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
    }

}//class
