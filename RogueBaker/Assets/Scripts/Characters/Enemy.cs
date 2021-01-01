using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool activated = true;
    public int health;
    public float moveSpeed;

    private Rigidbody2D rb;

    //to find and approach player
    public LayerMask whatIsPlayer;
    private float targetRadius;
    public float targetRadiusValue;
    private bool playerInRange;

    //for stun
    public float flashTime;
    Color originalColor;
    private SpriteRenderer spriteRenderer;
    public float stunTimerValue;
    private float stunTimer;
    bool stunned;

    //patrol variables
    bool patrollingToRight = true;
    float patrolInOneDirectionTime;
    public float patrolInOneDirectionTimeValue;

    //for shooting
    public float kamikazeeTime;
    private bool kamikazee = false;
    public float offset;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public Transform shootSpawn;
    public GameObject bullet;

    public GameObject deathEffect; //insert particle effect
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stunTimer = stunTimerValue;
        targetRadius = targetRadiusValue;
        patrolInOneDirectionTime = patrolInOneDirectionTimeValue;

        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, targetRadius, whatIsPlayer);

        Vector3 playerPosition = FindObjectOfType<PlayerController>().playerPosition;

        Vector3 difference = playerPosition - shootSpawn.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shootSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (!playerInRange && !GameController.instance.GameOver && !stunned)
        {
            Patrol();
        }

        if (activated)
        {
            if (playerInRange && !GameController.instance.GameOver && !stunned)
            {
                shootPlayer();
                kamikazeeTime -= Time.deltaTime;
            }
        }


        if (kamikazeeTime <= 0)
        {
            kamikazee = true;
            MoveToPlayer();
        }

        if (stunned)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunned = false;
                stunTimer = stunTimerValue;
            }
        }

        if (health <= 0)
        {
            Die();
        }
    }



    private void Patrol()
    {
        if (patrollingToRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            patrolInOneDirectionTime -= Time.deltaTime;

            if (patrolInOneDirectionTime <= 0)
            {
                patrollingToRight = false;
                patrolInOneDirectionTime = patrolInOneDirectionTimeValue;
            }
        }

        if (!patrollingToRight)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            patrolInOneDirectionTime -= Time.deltaTime;

            if (patrolInOneDirectionTime <= 0)
            {
                patrollingToRight = true;
                patrolInOneDirectionTime = patrolInOneDirectionTimeValue;
            }
        }

    }

    public void TakeDamage(int damage)
    {
        FlashRed();

        if (!stunned)
        {
            stunned = true;
        }

        health -= damage;
    }

    void FlashRed()
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }

    private void MoveToPlayer()
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().playerPosition;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, moveSpeed * Time.deltaTime);
    }

    void shootPlayer()
    {
        timeBetweenShots -= Time.deltaTime;

        if (timeBetweenShots <= 0)
        {
            Instantiate(bullet, shootSpawn.transform.position, shootSpawn.transform.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
    }

    void Die()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (kamikazee)
            {
                Die();
                GameController.instance.playerHurt();
            }
        }
    }
}
