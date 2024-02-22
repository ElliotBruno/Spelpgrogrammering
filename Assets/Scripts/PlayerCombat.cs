using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercombat : MonoBehaviour
{
   
    public Animator animator;
    private KeyCode meleeAttackKey = KeyCode.T;
    private KeyCode heavyAttackKey = KeyCode.R;
    public Transform attackpoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackdamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;



    void Update()
    { 
        if(Time.time > nextAttackTime) {
            if (Input.GetKeyDown(meleeAttackKey))
            {
                Attack();
                nextAttackTime = Time.time + 1f/attackRate;
            }
            if (Input.GetKeyDown(heavyAttackKey))
            {
                heavyattack();
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }

    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        Debug.Log("Hey");
        Collider2D[] hitenemies=Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemymovement>().TakeDamage(attackdamage);

        }
    }
    void heavyattack()
    {
        animator.SetTrigger("heavyattack");
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
