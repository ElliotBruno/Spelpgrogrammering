using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Playersprint : MonoBehaviour
{

    private Rigidbody2D rb;

    private Animator anim;

    private float dirX;

    private BoxCollider2D boxCol;

    private SpriteRenderer sprite;


    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jummping }

    private MovementState state = MovementState.idle;

    [SerializeField] private int JumpForce = 7;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int ExtraJumps = 1;
    [SerializeField] private int MaxJumps = 1;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Jump()
    {
        jumpSoundEffect.Play();
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
    }

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSoundEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed = 6;

        dirX = Input.GetAxisRaw("Horizontal");
        UpdateAnimationState();



        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();

        }
        else if (Input.GetButtonDown("Jump") && ExtraJumps > 0)
        {
            ExtraJumps--;
            DoubleJump();
        }

        if (isGrounded())
        {
            ExtraJumps = MaxJumps;
        }

    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && ExtraJumps == 0)
        {
            state = MovementState.double_jumping;
        }
        else if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }


        anim.SetInteger("State", (int)state);


    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer, jumpableGround);
        return raycastHit.collider != null;
    }
    /*
     * 
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0,new Vector2.(transform.local), 0.1f, groundLayer, jumpableGround);
        return raycastHit.collider != null;
    }
    */
}
