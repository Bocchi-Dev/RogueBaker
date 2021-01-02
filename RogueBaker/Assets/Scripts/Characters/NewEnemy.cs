using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;

    [Header("Stun Values")]
    public float flashTime;
    Color originalColor;
    private SpriteRenderer spriteRenderer;
    public float stunTimerValue;
    private float stunTimer;
    bool stunned;

    [Header("Patrol")]
    bool patrollingToRight = true;
    float patrolInOneDirectionTime;
    public float patrolInOneDirectionTimeValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        patrolInOneDirectionTime = patrolInOneDirectionTimeValue;
        stunTimer = stunTimerValue;
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned)
        {
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0)
            {
                stunned = false;
                stunTimer = stunTimerValue;
            }
        }

        if(!GameController.instance.GameOver && !stunned)
        {
            Patrol();
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
                Flip();
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
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    public void TakeDamage()
    {
        FlashRed();

        if (!stunned)
        {
            stunned = true;
        }
    }

    void FlashRed()
    {
        spriteRenderer.color = Color.white;
        Invoke("ResetColor", flashTime);
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.playerHurt();

            //knockback
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            //check if hit from left or right
            if (collision.transform.position.x < transform.position.x)
            {
                player.knockbackFromRight = true;
            }
            else
            {
                player.knockbackFromRight = false;
            }
        }
    }
}
