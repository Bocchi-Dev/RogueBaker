using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 5;
    public float jumpForce;

    private float horizontalMoveInput;
    private float verticalMoveInput;

    private Rigidbody2D rb;

    private bool facingRight = true;
    private bool isGrounded;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;


    public Vector3 playerPosition;

    [Header("Attacking stuff")]
    public int damage;
    public Transform hitPosition;
    public LayerMask whatIsEnemy;
    public float attackRange;
    private float timeBetweenAttacks = 0;
    public float startTimeBetweenAttacks;

    [Header("Animation Stuff")]
    public Animator animator;
    public bool jump = false;
    public UnityEvent OnLandEvent;
    public UnityEvent OnJumpEvent;
    

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!GameController.instance.ConversationActive)
        {
            horizontalMoveInput = Input.GetAxis("Horizontal");
            verticalMoveInput = Input.GetAxis("Vertical");
        }

        rb.velocity = new Vector2(horizontalMoveInput * moveSpeed, rb.velocity.y);

        if (isGrounded)
        {
            OnLandEvent.Invoke();
        }
        else
        {
            OnJumpEvent.Invoke();
        }


        if (GameController.instance.ConversationActive)
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
                OnLandEvent.Invoke();
            }
        }

        if (!facingRight && horizontalMoveInput > 0)
        {
            Flip();
        }
        else if (facingRight && horizontalMoveInput < 0)
        {
            Flip();
        }

        void Flip()
        {
            facingRight = !facingRight;

            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
        }
    }


    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMoveInput));

        //movement
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (!GameController.instance.ConversationActive)
        {
            if (Input.GetButtonDown("Jump") && extraJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
            }
            else if (Input.GetButtonDown("Jump") && extraJumps < 1 && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }

        timeBetweenAttacks -= Time.deltaTime;
        //attack
        if (timeBetweenAttacks <= 0)
        {
            if (!GameController.instance.ConversationActive)
            {
                if (Input.GetButtonDown("Attack"))
                {
                    timeBetweenAttacks = startTimeBetweenAttacks;
                    Attack();
                }
            }
        }

        //send position to playerPosition so enemy can find player
        playerPosition = new Vector2(transform.position.x, transform.position.y);
    }

    public void Attack()
    {
        if (isGrounded)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("AirAttack");
            animator.SetBool("AirAttacking", true);
        }
        
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(hitPosition.position, attackRange, whatIsEnemy);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].gameObject.tag == "Enemy")
            {
                FindObjectOfType<AudioManager>().Play("Hit");
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("AirAttacking", false);
    }

    public void OnJump()
    {
        animator.SetBool("IsJumping", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPosition.position, attackRange);
    }
}
