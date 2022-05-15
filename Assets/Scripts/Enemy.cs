using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;

    [SerializeField] private float rotationSpeed = 810f;

    private GameObject referenceCollider;
    private BoxCollider2D allowedArea;

    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;

    private Vector2 position;
    private Vector2 target;

    private void Start()
    {

        latestDirectionChangeTime = 0f;

        referenceCollider = GameObject.FindWithTag("AllowedArea");

        if (referenceCollider != null)
        {
            allowedArea = referenceCollider.GetComponent<BoxCollider2D>();
        }

        target = RandomPointInBounds(allowedArea.bounds);
    }

    void FixedUpdate()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        float step = moveSpeed * Time.deltaTime;

        position = transform.position;

        transform.position = Vector2.MoveTowards(position, target, step);

        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;

            target = RandomPointInBounds(allowedArea.bounds);
        }

        Vector2 movementDirection = new(target.x, target.y);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
    }

}//class
