using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemymovement : MonoBehaviour
{
    private float dirX;
    public Animator animator;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private GameObject[] waypoints;
    private Transform waypointTransform;
    [SerializeField] private float speed = 2f;



    private int currentWaypointIndex = 0;
    public int MaxHP = 100;
    int HP = 100;
/*    private int damage = 10;
*/

    private int enemycount = 5;
    private int points = 0;
    [SerializeField] private TextMeshProUGUI Enemycount;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        HP = MaxHP;

    }

    // Update is called once per frame
    void Update()
    {
        waypointTransform = waypoints[currentWaypointIndex].transform;
        if (Vector2.Distance(waypointTransform.position, transform.position) < .1f)
        {

            Debug.Log(currentWaypointIndex);
            currentWaypointIndex++;
            sprite.flipX = false;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                sprite.flipX = true;
                

            }

        }


     


   

        transform.position = Vector2.MoveTowards(transform.position, waypointTransform.position, Time.deltaTime * speed);


    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        animator.SetTrigger("Hit");
        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        Debug.Log("Died");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }
}

/*   void OnCollisionEnter2D(Collision2D collision) {

      if ((collision.gameObject.CompareTag("Player"))
      {
          Debug.Log(collision.gameObject.name);
          Damage(float damage);

      }

  }*/

/*  void Damage(float damage)
     {
         Debug.Log("tar skada :)");
         HP -= damage;
         if (HP <= 0)
         {
             Destroy(sprite.gameObject);
             points += 1;

             Enemycount.text = "Enemies Remaining: " + points;

             enemycount++;
         }



     }*/


