



using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Playermovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private Animator anim;

    private float dirX;

    private float horizontal;

    private BoxCollider2D boxCol;
/*    public BoxCollider2D box_Head;
 *    
*/  
    private CircleCollider2D box_Bottom;


    private SpriteRenderer sprite;
    private bool canDash = true;
    private bool isDashing;
    private float activespeed;
    public float dashspeed;
    public float dashlength = .5f, dashcooldown = 1f;
    private float dashcounter;
    private float dashcoolcounter;
    private float dashingcooldown=1f;
    private float dashingPower = 24f;
    private float dashTime = 0.2f;
    private KeyCode rollKey = KeyCode.V;


    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jummping, hurt, Roll }

    private MovementState state = MovementState.idle;

    [SerializeField] private int JumpForce = 7;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private int ExtraJumps = 1;
    [SerializeField] private int MaxJumps = 1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallcheck;

/*    [SerializeField] private TrailRenderer tr;
*/
/*    private bool wallslide;
*/    
    private float wallslidespeed = 2f;
    private bool walljumping;
    private float wallJumpCooldown;

    private float walljumpdirection;
    private float walljumptime=0.2f;
    private float walljumpcounter;
    private float walljumpduration=0.4f;
    private Vector2 walljumppower = new Vector2(8f, 16f);
    public GameObject portal;
    public GameObject rickMorty;






    /*    private void Wallslide()
        {
            if (Iswall() && !isGrounded() && horizontal != 0f)
            {
                wallslide = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallslidespeed, float.MaxValue));
            }
            else
            {
                wallslide = false;
            }
        }*/
    private void Jump()
    {
        if (isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
        else if( Wall() && !isGrounded())
        {
            if (horizontal==0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
            wallJumpCooldown = 0;
            rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)* 3,6);
        }
     
        //rb.AddForce(new Vector2(0, 1f) * JumpForce * Time.deltaTime);
    }

    private void DoubleJump()
    {
        jumpSoundEffect.Play();

        rb.velocity = new Vector2(rb.velocity.x, JumpForce);

        //rb.AddForce(new Vector2(0, 1f) * JumpForce * Time.deltaTime);
    }

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSoundEffect;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        box_Bottom = GetComponent<CircleCollider2D>();


        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        activespeed = moveSpeed;
        Invoke("SpawnDelay",3);

    }
/*    private IEnumerator void SpawnDelay()
    {
        yield return
        portal.SetActive(false);
        rickMorty.SetActive(true);
    }*/

    private void Roll()
    {



        if (dirX > -1f)
        {
            anim.SetTrigger("Roll");

            transform.position = transform.position + new Vector3(10, 0, 0);
            sprite.flipX = false;
        }
       
     /*   while (sprite.flipX=false)
        {
            transform.position = transform.position + new Vector3(10, 0, 0);

        }
        while (sprite.flipX = true)
        {
            transform.position = transform.position + new Vector3(10, 0, 0);

        }*/
        if (dirX > 0f)
        {
            anim.SetTrigger("Roll");

            transform.position = transform.position + new Vector3(10, 0, 0);
            sprite.flipX = false;


        }
        else if (dirX < 0)
        {
            sprite.flipX = true;
            transform.position = transform.position + new Vector3(-10, 0, 0);


        }
        else
        {


        }

        /*        yield return new WaitForSeconds(dashcooldown);
        */
        /*        boxCol.enabled = true;
        */
        /*
                canDash = false;
                isDashing = true;
                float orginalGravity = rb.gravityScale;
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
                tr.emitting = false;
                rb.gravityScale = orginalGravity;
                isDashing = false;
                yield return new WaitForSeconds(dashcooldown);
                canDash = true;

                if (dashcoolcounter <= 0 && dashcounter <= 0)
                {
                    activespeed = dashspeed;
                    dashcounter = dashlength;
                }*/


    }

    // Update is called once per frame

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        
        if (Input.GetKeyDown(rollKey))
        {
            //Debug.Log("hej");
            Roll();
        }





        anim.SetBool("run", horizontal != 0);
        anim.SetBool("ground", isGrounded());

        if (Input.GetKeyDown(rollKey))
        {
/*            StartCoroutine(Roll());
*/
            if (dashcoolcounter<=0 && dashcounter<=0)
            {
                activespeed = dashspeed;
                dashcounter = dashlength;
            }
        }
        if (dashcounter>0)
        {
            dashcounter -= Time.deltaTime;
            if (dashcounter<=0)
            {
                activespeed = moveSpeed;
                dashcoolcounter = dashcooldown;
            }
            if (dashcoolcounter>0)
            {
                dashcoolcounter -= Time.deltaTime;
            }
        }
        if (dashcoolcounter>0)
        {
            dashcoolcounter -= Time.deltaTime;
        }
        /*        Wallslide();
        */
        dirX = Input.GetAxisRaw("Horizontal");
        UpdateAnimationState();



        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            Jump();
            anim.SetBool("grounded", isGrounded());


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
        if (wallJumpCooldown < 0.2f)
        {
            
                rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            
            if (Wall() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
                rb.gravityScale = 3;
            if (Input.GetKey(KeyCode.Space))

                Jump();
        }
        else wallJumpCooldown += Time.deltaTime;

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
            sprite.flipX = true;
            state = MovementState.running;

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
        if (Wall()==true)
        {
            state = MovementState.wall_jummping;

        }
      




        anim.SetInteger("State", (int)state);


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((collision.gameObject.CompareTag("Spikes")) || (collision.gameObject.CompareTag("SpikeMan")) || (collision.gameObject.CompareTag("Bottom")) || collision.gameObject.CompareTag("Enemy"))
        {
            state = MovementState.hurt;



        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool Wall()
    {

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}



    /*  private bool isGrounded()
      {
          return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 1f, jumpableGround);
      }*/
    /* private bool IsGrounded()
     {
         return false;
     }*/









/*    private void walljump()
    {
        if (wallslide)
        {
            walljumping = true;
            walljumpdirection = -transform.localScale.x;
            walljumpcounter = walljumptime;
        }
        else
        {
            walljumpcounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && walljumpcounter >0f)
        {
            walljumping = true;
            rb.velocity=new Vector2(walljumpdirection * walljumppower.x, walljumppower.y)
            walljumpcounter = 0f;
            if (transform.localScale.x != walljumpdirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale=localScale;
            }
            Invoke(nameof(stopwalljump), walljumpduration);
        }
    }

    private void stopwalljump()
    {
        walljumping = false;
    }*/

/*
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("ground", isGrounded());
        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}

*/

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Playermovement : MonoBehaviour
{

    private Rigidbody2D rb;

    private Animator anim;

    private float dirX;

    private BoxCollider2D boxCol;

    private SpriteRenderer sprite;

    private float wallJumpCooldown; 
    private enum MovementState {idle,running,jumping,falling,double_jumping,wall_jummping}

    private MovementState state = MovementState.idle;

 

    [SerializeField] private int JumpForce = 7;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private float jumpPower;
    [SerializeField] private int ExtraJumps = 1;
    [SerializeField] private int MaxJumps = 1;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Jump()
    {
        if(isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        else if (onWall() && !isGrounded())
        {
            if(dirX  == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Math.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            
            wallJumpCooldown = 0;

        }
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

        if(isGrounded())
        {
            ExtraJumps = MaxJumps;
        }

        if (wallJumpCooldown > 0.2f)
        {
            if (Input.GetKey(KeyCode.Space))
                Jump();

            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (onWall() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else rb.gravityScale = 1;
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirX<0)
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
        else if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }


        anim.SetInteger("State", (int)state);


    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
        return raycastHit.collider != null;
    } 
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0,new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
*/







/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRend;
    private BoxCollider2D boxCol;
    private SpriteRenderer sprite;


    [Header("Ground collision")]
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private int maxJumps = 1;

    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jumping }
    private MovementState state = MovementState.idle;

    [Header("Audio")]
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource doubleJumpSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Move();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
        else if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            extraJumps--;
            DoubleJump();
        }

        UpdateAnimationState();


        if (IsGrounded())
        {
            extraJumps = maxJumps;
        }

    }
    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }
    private void Move()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (dirX > 0)
        {
            spriteRend.flipX = false;
        }
        else if (dirX < 0)
        {
            spriteRend.flipX = true;
        }

    }

    private void Jump()
    {
        jumpSoundEffect.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        doubleJumpSoundEffect.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    private void UpdateAnimationState()
    {

        if (dirX > 0f)
        {
            state = MovementState.running;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f && extraJumps == 0)
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

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
*/


