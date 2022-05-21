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

    private bool shieldActive;

    [SerializeField] private GameObject particlesReference;
    private GameObject spawnedDeathParticles;

    private GameObject gameOverScreen;

    private GameObject canvasChildObject;
    private GameObject imageChildObject;

    private Image imageChild;

    private Vector3 originalShieldPosition;
    private Quaternion originalShieldRotation;

    private Color shieldColor;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundEffect = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        instance = this;

        canMove = true;
        canTrigger = true;

        playerDead = false;

        shieldActive = false;

        gameOverScreen = FindInActiveObjectByTag("GameOverScreen");

        SetInvisibleShieldOnPlayer();

        GetShieldIconPosition();

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

        canvasChildObject.transform.localPosition = originalShieldPosition;
        canvasChildObject.transform.rotation = originalShieldRotation;

        ChangeShieldVisibility();

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

                    spawnedDeathParticles = Instantiate(particlesReference);
                    spawnedDeathParticles.transform.position = collision.gameObject.transform.position;

                    Destroy(collision.gameObject); //Destroys enemy

                    Destroy(spawnedDeathParticles, 2f); //Destroys particles after 2 seconds;
                }

                if (collision.gameObject.transform.localScale.magnitude > transform.localScale.magnitude)
                {
                    //Debug.Log("Enemy BIGGER than Player");
                    if (shieldActive == true)
                    {
                        PlayAudio("shieldBreak", 0);
                        shieldActive = false;
                    }
                    else
                    {
                        PlayAudio("deathSound", 0);

                        TimerController.instance.EndTimer();

                        if (gameOverScreen != null) gameOverScreen.SetActive(true);

                        playerDead = true;

                        spriteRenderer.enabled = false;
                        canMove = false;
                        canTrigger = false;
                    }
                }
            }
            if (collision.gameObject.CompareTag(POWERUP_TAG))
            {
                PlayAudio("powerUp", 0);

                switch (collision.gameObject.name)
                {
                    case "PowerUp01(Clone)":
                        Debug.Log("Picked Up Bloody Whirl");
                        Enemy.isBloodyWhirlActive = true;
                        Destroy(collision.gameObject);
                        Invoke(nameof(SetBoolFalse), 0.8f);
                        break;
                    case "PowerUp02(Clone)":
                        Debug.Log("Picked Up Boost+++");
                        if (moveSpeed < 5.2f) moveSpeed += 1.8f;
                        else moveSpeed = 7f;
                        Destroy(collision.gameObject);
                        break;
                    case "PowerUp03(Clone)":
                        Debug.Log("Picked Up Boost++");
                        if (moveSpeed < 5.8f) moveSpeed += 1.2f;
                        else moveSpeed = 7f;
                        Destroy(collision.gameObject);
                        break;
                    case "PowerUp04(Clone)":
                        Debug.Log("Picked Up Boost+");
                        if (moveSpeed < 6.3f) moveSpeed += 0.7f;
                        else moveSpeed = 7f;
                        Destroy(collision.gameObject);
                        break;
                    case "PowerUp05(Clone)":
                        Debug.Log("Picked Up Beetle Shell");
                        shieldActive = true;
                        Destroy(collision.gameObject);
                        break;
                    case "PowerUp06(Clone)":
                        Debug.Log("Picked Up Oblivion Rose");
                        killCount += 50;
                        Destroy(collision.gameObject);
                        break;
                    default:
                        Debug.Log("How did this happen!?");
                        break;
                }
            }
        }
    }

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

    private void SetInvisibleShieldOnPlayer()
    {
        canvasChildObject = this.transform.GetChild(0).gameObject;
        imageChildObject = canvasChildObject.transform.GetChild(0).gameObject;

        imageChild = imageChildObject.GetComponent<Image>();

        shieldColor = imageChild.color;
        shieldColor.a = 0f;
        imageChild.color = shieldColor;
    }

    private void GetShieldIconPosition()
    {
        originalShieldPosition = canvasChildObject.transform.localPosition;
        originalShieldRotation = canvasChildObject.transform.rotation;
    }

    private void ChangeShieldVisibility()
    {
        if (shieldActive == true)
        {
            shieldColor.a = 1f;
            imageChild.color = shieldColor;
        }
        else
        {
            shieldColor.a = 0f;
            imageChild.color = shieldColor;
        }
    }

    private void SetBoolFalse()
    {
        Enemy.isBloodyWhirlActive = false;
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
