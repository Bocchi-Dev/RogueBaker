using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    public Animator animator;
    public float moveSpeed = 5;
    public float jumpForce;
    public float fastFall;

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

    [Header("Attacking Variables")]
    private float timeBetweenAttacks = 0;
    public float startTimeBetweenAttacks;
    public GameObject weapon;

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

        if (GameController.instance.ConversationActive)
        {
            rb.velocity = new Vector2(0, -fastFall);
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
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
            else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }

        if (!isGrounded && verticalMoveInput < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalMoveInput * fastFall);
        }

        //attacking
        if (timeBetweenAttacks <= 0)
        {
            if (!GameController.instance.ConversationActive)
            {
                if (Input.GetButtonDown("Attack"))
                {
                    weapon.GetComponent<SwordAttack>().Attack();
                }
            }
        }

        //send position to playerPosition so enemy can find player
        playerPosition = new Vector2(transform.position.x, transform.position.y);
    }
}
