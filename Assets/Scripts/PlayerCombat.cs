using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercombat : MonoBehaviour
{
   
    public Animator animator;
    private KeyCode meleeAttackKey = KeyCode.T;
    private KeyCode heavyAttackKey = KeyCode.R;


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
    }
    void heavyattack()
    {
        animator.SetTrigger("heavyattack");
    }
}
