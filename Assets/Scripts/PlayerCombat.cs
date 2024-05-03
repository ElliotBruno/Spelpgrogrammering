using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Playercombat : MonoBehaviour
{
   
    public Animator animator;
    private KeyCode meleeAttackKey = KeyCode.T;
    private KeyCode heavyAttackKey = KeyCode.R;
    public Transform attackpoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackdamage = 70;
    public int heavydamage = 100;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    private SpriteRenderer sprite;
    private float dirX;


    private MovementState state = MovementState.idle;

    private enum MovementState { idle, running, jumping, falling, double_jumping, wall_jummping, hurt, Roll }



    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

    }
    void Update()
    { 
        if(Time.time > nextAttackTime) {
            if (Input.GetKeyDown(meleeAttackKey))
            {
                Attack();
                SlashSoundEffect.Play();

                nextAttackTime = Time.time + 1f/attackRate;
            }
            if (Input.GetKeyDown(heavyAttackKey))
            {
                heavyattack();
                AttackSoundEffect.Play();

                nextAttackTime = Time.time + 1f/attackRate;
            }
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
                FlipAttackPoint(false);

            }
            else if (dirX < 0)
            {
                state = MovementState.running;
                sprite.flipX = true;
                FlipAttackPoint(true);
            }
        }

    }
    [Header("Audio")]
    [SerializeField] private AudioSource AttackSoundEffect;
    [SerializeField] private AudioSource SlashSoundEffect;
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

    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitenemies=Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemymovement>().TakeDamage(attackdamage);

        }
    }
    void heavyattack()
    {
        animator.SetTrigger("heavyattack");

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemymovement>().TakeDamage(heavydamage);

        }
    }

    private void FlipAttackPoint(bool flip)
    {
        if (flip)
        {
           
            attackpoint.localScale = new Vector3(-4f, 1f, 1f); 
        }
        else
        {
            
            attackpoint.localScale = new Vector3(1f, 1f, 1f); 
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackpoint==null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
}
