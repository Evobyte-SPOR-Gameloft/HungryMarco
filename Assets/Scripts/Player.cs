using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float rotationSpeed = 700f;

    private float movementX;
    private float movementY;

    private Rigidbody2D playerBody;

    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private string CRAWL_ANIMATION = "isCrawling";



    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMovement();
        PlayerAnimation();
    }

    void FixedUpdate()
    {

    }

    void PlayerMovement()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        Vector2 movementDirection = new Vector2(movementX, movementY);

        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);

        if(movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void PlayerAnimation()
    {
        if(movementX != 0 || movementY != 0) animator.SetBool(CRAWL_ANIMATION, true);
        else animator.SetBool(CRAWL_ANIMATION, false);
    }

}//class

