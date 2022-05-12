using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 810f;

    [SerializeField]
    private float animationMultiplier = 0.2f;

    [HideInInspector]
    public float moveSpeed = 3.0f;

    private Rigidbody2D enemyBody;

    private float movementX;
    private float movementY;

    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;

    private Vector2 movementDirection;

    private void Awake()
    {

        enemyBody = GetComponent<Rigidbody2D>();

        latestDirectionChangeTime = 0f;
    }
    //void Start()
    //{
    //
    //}

    private void Update()
    {
        EnemyMovement();
    }

    void FixedUpdate()
    {

    }
    void EnemyMovement()
    {
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;

            movementX = Random.Range(-1, 1);
            movementY = Random.Range(-1, 1);

            movementDirection = new(movementX, movementY);

            movementDirection.Normalize();
        }

        enemyBody.velocity = movementDirection;

        //float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        //transform.Translate(inputMagnitude * moveSpeed * Time.deltaTime * movementDirection, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

}//class
