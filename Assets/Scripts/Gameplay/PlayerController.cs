using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // public GameManager gameManager;

    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;
    [SerializeField] GameObject slideDust;


    private PlayerSensor groundSensor;
    private PlayerSensor wallSensorR1;
    private PlayerSensor wallSensorR2;
    private PlayerSensor wallSensorL1;
    private PlayerSensor wallSensorL2;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isWallSliding = false;
    private bool grounded = false;
    private bool rolling = false;
    private int facingDirection = 1;
    private float currentAttack;
    private float timeSinceAttack = 0.0f;
    public float cooldown = 1f;
    private float lastAttackedAt = -9999f;
    private float delayToIdle = 0.0f;
    private float rollDuration = 0.0f;
    private float rollCurrentTime;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLaters;
    public int attackDamage = 20;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;


    // public TextMeshProUGUI healthText;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerSensor>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<PlayerSensor>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerSensor>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerSensor>();

        // if (GameManager.Instance != null)
        // {
        //     currentHealth = GameManager.Instance.currentHealth;
        // }
        // else
        // {
        // }
        // healthText.text = "Hero: " + currentHealth.ToString();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // healthBar.SetHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (rolling)
        {
            rollCurrentTime += Time.deltaTime;
        }

        // Disable rolling if timer extends duration
        if (rollCurrentTime > rollDuration)
        {
            rolling = false;
        }

        // Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        // Check if character just started failling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // handle input and movement
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
            // handle attack direction
            attackPoint.localPosition = new Vector3(0.5f, attackPoint.localPosition.y, 0);
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
            // handle attack direction
            attackPoint.localPosition = new Vector3(-0.5f, attackPoint.localPosition.y, 0);
        }

        // Move
        if (!rolling)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }

        // Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        // Wall Slide
        isWallSliding = (wallSensorR1.State() && wallSensorR2.State()) || (wallSensorL1.State() && wallSensorL2.State());
        animator.SetBool("WallSlide", isWallSliding);

        // Death
        if (Input.GetKeyDown("e") && !rolling)
        {
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");
        }
        // Hurt
        else if (Input.GetKeyDown("q") && !rolling)
        {
            animator.SetTrigger("Hurt");
        }
        // Attack
        else if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !rolling)
        {
            if (Time.time > lastAttackedAt + cooldown)
            {
                Attack();
            }
        }
        // Block
        // else if (Input.GetMouseButtonDown(1) && !rolling)
        // {
        //     animator.SetTrigger("Block");
        //     animator.SetBool("IdleBlock", true);
        // }
        // else if (Input.GetMouseButtonUp(1))
        // {
        //     animator.SetBool("IdleBlock", false);

        // }
        // Roll
        else if (Input.GetKeyDown("left shift") && !rolling && !isWallSliding)
        {
            rolling = true;
            animator.SetTrigger("Roll");
            rb.velocity = new Vector2(facingDirection * rollForce, rb.velocity.y);
        }
        // Jump
        else if (Input.GetKeyDown("space") && grounded && !rolling)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        // Idle
        else
        {
            // prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0)
            {
                animator.SetInteger("AnimState", 0);
            }

        }

    }

    void Respawn()
    {
        // gameManager.DoSlowmotion();
        GameManager.Instance.DoSlowmotion();

        currentHealth = maxHealth;
        // healthText.text = "Hero: " + currentHealth.ToString();
        GameManager.Instance.currentHealth = currentHealth;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Respawn");
    }

    public void Attack()
    {
        currentAttack++;

        // Loop back to one after third attack
        if (currentAttack > 3)
        {
            currentAttack = 1;
        }

        // Reset attack combo if time since last attack is too large
        // if (timeSinceAttack > 1.0f)
        // {
        //     currentAttack = 1;
        // }

        // Call on of three animations "Attack1", "Attack2", "Attack3"
        animator.SetTrigger("Attack" + currentAttack);

        // play attack sound
        FindObjectOfType<AudioManager>().Play("sword-attack-player");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLaters);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        // Reset timer
        timeSinceAttack = 0.0f;

        // Update last attack time
        lastAttackedAt = Time.time;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        // healthText.text = "Hero: " + currentHealth.ToString();
        GameManager.Instance.currentHealth = currentHealth;
        healthBar.SetHealth(currentHealth);

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= 30)
        {
            Respawn();
        }

    }

    void Die()
    {
        animator.SetTrigger("Death");

        // GetComponent<Collider2D>().enabled = false;
        enabled = false;
        // destroy the enemy after 1 second
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.CompareTag("AttackSlash"))
        // {
        //     Debug.Log("Player hit by enemy");
        // }

        // if (collision.name == "Checkpoint_1")
        // {
        //     GameManager.Instance.GoLevel2();
        // }

        // if (collision.name == "Checkpoint_2")
        // {
        //     // GameManager.Instance.GoLevel3();
        //     GameManager.Instance.GoMainMenu();
        // }
    }

    // Draw attack range in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Animation events
    // Called in slide animation
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (facingDirection == 1)
        {
            spawnPosition = wallSensorR2.transform.position;
        }
        else
        {
            spawnPosition = wallSensorL2.transform.position;
        }

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation);
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
