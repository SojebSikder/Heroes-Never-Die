using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isBoss = false;
    private Transform player;
    public bool isFlipped = false;


    private Animator animator;
    public int maxHealth = 200;
    int currentHealth;
    public HealthBar EnemyhealthBar;

    // when emeny hit player, player health will be decrease by 20
    public int attackDamage = 20;


    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;


    // public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;

        currentHealth = maxHealth;
        // healthText.text = "Villain: " + currentHealth.ToString();
        if (isBoss)
        {
            EnemyhealthBar.SetMaxHealth(maxHealth);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        // healthText.text = "Villain: " + currentHealth.ToString();
        if (isBoss)
        {
            currentHealth -= damage;
            EnemyhealthBar.SetHealth(currentHealth);
            // randome between 2 hit animations for boss
            animator.SetTrigger("Hit_" + Random.Range(1, 3));
        }
        else
        {
            currentHealth -= damage * 2;
            // for skeleton enemy
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        // destroy the enemy after 1 second
        Destroy(gameObject, 2f);
    }


    public void AttackStart()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);

        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    public void AttackStartEffectObject()
    {
        // Debug.Log ("Fire Effect Object");
    }
}
