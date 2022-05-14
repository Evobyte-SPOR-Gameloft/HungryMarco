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

    //private CharacterController controller;
    private Rigidbody2D playerBody;

    private Animator animator;
    private readonly string crawlAnimation = "isCrawling";
    private readonly string crawlAnimationSpeedMultiplier = "crawlMultiplier";



    void Awake()
    {
        //controller = GetComponent<CharacterController>();
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

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
        Vector2 move = new(movementX, movementY);

        move.Normalize();

        //controller.Move(moveSpeed * Time.deltaTime * move);

        playerBody.MovePosition(playerBody.position + (move * moveSpeed * Time.fixedDeltaTime));

        Vector2 movementDirection = new(movementX, movementY);

        if (movementDirection != Vector2.zero)
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

