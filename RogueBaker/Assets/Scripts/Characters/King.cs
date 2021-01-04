using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    public float health;
    public float moveSpeed = 10;
    public bool damageable = false;

    private Rigidbody2D rb;

    //for stun
    public float flashTime;
    Color originalColor;
    private SpriteRenderer spriteRenderer;
    private Animator anime;

    //shooting
    public float offset;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public Transform shootSpawn;
    public GameObject bullet;
    public LayerMask whatIsPlayer;
    private float targetRadius;
    public float targetRadiusValue;
    private bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();

        targetRadius = targetRadiusValue;

        originalColor = spriteRenderer.color;
        health = GameController.instance.kingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //king will only start attacking when it is bossfight time
        if (GameController.instance.bossFightTime && !GameController.instance.ConversationActive)
        {
            damageable = true;

            anime.SetBool("BossFightTime", true);
            if (health <= 0)
            {
                Die();
            }

            if (playerInRange && !GameController.instance.GameOver && damageable)
            {
                shootPlayer();
            }
        }

        Vector3 playerPosition = FindObjectOfType<PlayerController>().playerPosition;

        Vector3 difference = playerPosition - shootSpawn.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shootSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        playerInRange = Physics2D.OverlapCircle(transform.position, targetRadius, whatIsPlayer);

        
    }

    private void Die()
    {
        Destroy(gameObject);
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

    public void TakeDamage()
    {
        if (damageable)
        {
            FlashRed();
            health -= 1;
        }
    }

    void shootPlayer()
    {
        timeBetweenShots -= Time.deltaTime;
        anime.SetTrigger("Attack");
        if (timeBetweenShots <= 0)
        {
            Instantiate(bullet, shootSpawn.transform.position, shootSpawn.transform.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }
}
