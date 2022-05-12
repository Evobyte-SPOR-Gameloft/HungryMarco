using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.0f;

    [SerializeField]
    private float rotationSpeed = 810f;

    [SerializeField]
    private float animationMultiplier = 0.2f;

    private float movementX;
    private float movementY;

    private Rigidbody2D playerBody;

    //private SpriteRenderer spriteRenderer;

    private Animator animator;
    private readonly string crawlAnimation = "isCrawling";
    private readonly string crawlAnimationSpeedMultiplier = "crawlMultiplier";



    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();

        //spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
    }

    //void Start()
    //{
    //    
    //}

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        PlayerMovement();
        PlayerAnimation();
    }

    void PlayerMovement()
    {
        //movementX = Input.GetAxisRaw("Horizontal");
        //movementY = Input.GetAxisRaw("Vertical");

        Vector2 movementDirection = new(movementX, movementY);

        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        movementDirection.Normalize();

        playerBody.transform.Translate(inputMagnitude * moveSpeed * Time.deltaTime * movementDirection, Space.World);


        if(movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void PlayerAnimation()
    {
        if (movementX != 0 || movementY != 0)
        {
            animator.SetBool(crawlAnimation, true);
            animator.SetFloat(crawlAnimationSpeedMultiplier, (moveSpeed * animationMultiplier));
        }
        else animator.SetBool(crawlAnimation, false);

    }

}//class

