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



    void Update()
    { 
        if (Input.GetKeyDown(meleeAttackKey))
        {
            Attack();
        }
        if (Input.GetKeyDown(heavyAttackKey))
        {
            heavyattack();
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
