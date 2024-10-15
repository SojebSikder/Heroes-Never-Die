using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float attackRange = 1f;
    public int attackDamage = 20;
    public LayerMask attackMask;

    private Animator animator;
    private bool isFlipped = true;


    public GameObject ray;


    // public float smoothSpeed = 0.125f;
    // public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        Move();

        if (player.GetComponent<PlayerController>().currentHealth <= 40)
        {
            EnableRay();
        }
        // if (Vector2.Distance(transform.position, player.position) < attackRange)
        // {
        //     Attack();
        // }
    }

    // Enable ray for 5 seconds
    public void EnableRay()
    {
        ray.SetActive(true);
        // disable ray after 8 seconds
        StartCoroutine(DisableRay());
    }

    IEnumerator DisableRay()
    {
        yield return new WaitForSeconds(5f);
        ray.SetActive(false);
    }

    void LookAtPlayer()
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

    void Move()
    {
        // top of player head
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x - 0.7f, player.position.y + 3.7f), speed * Time.deltaTime);
        // animator.SetFloat("Speed", speed);
    }

    void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Enemy>().TakeDamage(attackDamage);

            animator.SetTrigger("Attack");

            speed = 0;

            StartCoroutine(ResetSpeed());
        }
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(1f);
        speed = 2f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
        Destroy(gameObject, 1f);
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");

        if (damage >= 100)
        {
            Die();
        }
    }

    public void AttackStart()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, attackMask);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Enemy>().TakeDamage(attackDamage);

            animator.SetTrigger("Attack");

            speed = 0;
            StartCoroutine(ResetSpeed());
        }
    }


}
