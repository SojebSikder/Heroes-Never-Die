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

        // if (Vector2.Distance(transform.position, player.position) < attackRange)
        // {
        //     Attack();
        // }
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
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x - 0.5f, player.position.y + 1.8f), speed * Time.deltaTime);
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
