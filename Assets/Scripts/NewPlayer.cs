using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float rotationSpeed = 810f;

    [SerializeField] private float animationMultiplier = 0.2f;

    [SerializeField] private float collisionOffset = 0.05f;

    [SerializeField] private ContactFilter2D movementFilter;

    public static NewPlayer instance;

    private Vector2 moveInput;
    private Rigidbody2D playerBody;
    private AudioSource soundEffect;
    private SpriteRenderer spriteRenderer;

    private Vector2 directionOfRotation;
    private Animator animator;
    private readonly string crawlAnimation = "isCrawling";
    private readonly string crawlAnimationSpeedMultiplier = "crawlMultiplier";

    private readonly List<RaycastHit2D> castCollisions = new();

    private readonly string ENEMY_TAG = "Enemy";
    private readonly string POWERUP_TAG = "PowerUp";

    [HideInInspector] public int killCount = 0;
    [HideInInspector] public bool playerDead;

    private bool canMove;
    private bool canTrigger;

    private GameObject gameOverScreen;

    private void Awake()
    {
        instance = this;

        canMove = true;
        canTrigger = true;

        playerDead = false;

        gameOverScreen = FindInActiveObjectByTag("GameOverScreen");
    }
    public void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundEffect = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        // playerBody.MovePosition(playerBody.position + (moveInput * moveSpeed * Time.fixedDeltaTime));


        // Try to move player in input direction, followed by left right and up down input if failed
        bool success = MovePlayer(moveInput);

        if (!success)
        {
            // Try Left / Right
            success = MovePlayer(new Vector2(moveInput.x, 0));

            if (!success)
            {
                #pragma warning disable IDE0059 // Unnecessary assignment of a value
                success = MovePlayer(new Vector2(0, moveInput.y));

            }
        }

        directionOfRotation = new(moveInput.x, moveInput.y);

        if (directionOfRotation != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, directionOfRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        PlayerAnimation();

    }

    // Tries to move the player in a direction by casting in that direction by the amount
    // moved plus an offset. If no collisions are found, it moves the players
    // Returns true or false depending on if a move was executed
    public bool MovePlayer(Vector2 direction)
    {
        if (canMove)
        {
            // Check for potential collisions
            int count = playerBody.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                Vector2 moveVector = moveSpeed * Time.fixedDeltaTime * direction;

                // No collisions
                playerBody.MovePosition(playerBody.position + moveVector);

                return true;
            }
            else
            {
                // Print collisions
                foreach (RaycastHit2D hit in castCollisions)
                {
                    print(hit.ToString());
                }

                return false;
            }

        }
        else return false;
    }

    void PlayerAnimation()
    {
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            animator.SetBool(crawlAnimation, true);
            animator.SetFloat(crawlAnimationSpeedMultiplier, (moveSpeed * animationMultiplier));
        }
        else animator.SetBool(crawlAnimation, false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {
            if (collision.gameObject.CompareTag(ENEMY_TAG))
            {
                if (collision.gameObject.transform.localScale.magnitude < transform.localScale.magnitude)
                {
                    //Debug.Log("Enemy SMALLER than Player");

                    transform.localScale += collision.gameObject.transform.localScale / 100; //Adds 1/100 of the enemy scale to the player's scale

                    killCount++;
                    PlayAudio("crunchSound", 0);
                    Destroy(collision.gameObject); //Destroys enemy
                }

                if (collision.gameObject.transform.localScale.magnitude > transform.localScale.magnitude)
                {
                    //Debug.Log("Enemy BIGGER than Player");

                    PlayAudio("deathSound", 0);

                    TimerController.instance.EndTimer();

                    if (gameOverScreen != null) gameOverScreen.SetActive(true);

                    playerDead = true;

                    spriteRenderer.enabled = false;
                    canMove = false;
                    canTrigger = false;
                }
            }
            if (collision.gameObject.CompareTag(POWERUP_TAG))
            {
                PlayAudio("powerUp", 0);
                Destroy(collision.gameObject);
            }
        }
    }

    //To be removed when switching to events and delegates for sound
    void PlayAudio(string filename, ulong delay)
    {
        soundEffect.clip = Resources.Load<AudioClip>("Audioclips/" + filename);
        soundEffect.Play(delay);
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnFire()
    {
        print("Pew Pew...");
    }

}//class
