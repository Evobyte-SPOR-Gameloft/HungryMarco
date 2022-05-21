using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject canvasChildObject;
    private GameObject imageChildObject;

    private Image imageChild;

    private Vector3 originalSkullPosition;
    private Quaternion originalSkullRotation;

    private GameObject playerObject;
    private Color skullColor;

    public static bool isBloodyWhirlActive = false;

    private Renderer enemyRenderer;


    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();

        SetInvisibleSkullOnEnemies();

        GetSkullIconPosition();

        SelectPointToMoveTo();
    }

    void FixedUpdate()
    {
        EnemyMovement();

        canvasChildObject.transform.localPosition = originalSkullPosition;
        canvasChildObject.transform.rotation = originalSkullRotation;

        ChangeSkullVisibility();

        MoveTowardsPlayerIfPowerUpActive();

        Invoke(nameof(SelfDestruct), 30f);
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

    private static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
    }

    private void SelectPointToMoveTo()
    {
        latestDirectionChangeTime = 0f;

        referenceCollider = GameObject.FindWithTag("AllowedArea");

        if (referenceCollider != null)
        {
            allowedArea = referenceCollider.GetComponent<BoxCollider2D>();
        }

        target = RandomPointInBounds(allowedArea.bounds);
    }

    private void SetInvisibleSkullOnEnemies()
    {
        canvasChildObject = this.transform.GetChild(0).gameObject;
        imageChildObject = canvasChildObject.transform.GetChild(0).gameObject;

        imageChild = imageChildObject.GetComponent<Image>();

        skullColor = imageChild.color;
        skullColor.a = 0f;
        imageChild.color = skullColor;
    }

    private void ChangeSkullVisibility()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            playerObject = GameObject.FindWithTag("Player");

            if (transform.localScale.magnitude > playerObject.transform.localScale.magnitude)
            {
                skullColor.a = 1f;
                imageChild.color = skullColor;
            }
            else
            {
                skullColor.a = 0f;
                imageChild.color = skullColor;
            }
        }
    }

    private void GetSkullIconPosition()
    {
        originalSkullPosition = canvasChildObject.transform.localPosition;
        originalSkullRotation = canvasChildObject.transform.rotation;
    }

    void MoveTowardsPlayerIfPowerUpActive()
    {

        if (isBloodyWhirlActive == true)
        {
            if (transform.localScale.magnitude < playerObject.transform.localScale.magnitude)
            {
                transform.position = Vector3.Lerp(this.transform.position, playerObject.transform.position, 10f * Time.deltaTime);
            }
        }

    }

    void SelfDestruct()
    {
        if(enemyRenderer.isVisible == false)
        Destroy(gameObject);
    }

}//class
