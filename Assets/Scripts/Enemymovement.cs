using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Enemymovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    itemcollector itemcollector;


    public int enemyCount = 1;

    [SerializeField] private TextMeshProUGUI Enemytext;

    private float dirX;
    private BoxCollider2D boxCol;
    [SerializeField] private LayerMask jumpableGround;

    public Animator animator;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool levelCompleted;
    private AudioSource finishSound;



    [SerializeField] private GameObject[] waypoints;
    private Transform waypointTransform;
    [SerializeField] private float speed = 2f;




    private int currentWaypointIndex = 0;
    public int MaxHP = 300;
    int HP = 300;

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");

        }


    }

    void Start()
    {
        anim = GetComponent<Animator>();
        finishSound = GetComponent<AudioSource>();

        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        HP = MaxHP;
        itemcollector = player.GetComponent<itemcollector>();


    }

    // Update is called once per frame
    void Update()
    {
        waypointTransform = waypoints[currentWaypointIndex].transform;
        if (Vector2.Distance(waypointTransform.position, transform.position) < .1f)
        {

            currentWaypointIndex++;
            sprite.flipX = false;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                sprite.flipX = true;


            }
            //Debug.Log($"enemy count {enemyCount}, points {itemcollector.points} levelcomp {levelCompleted}");
           /* if (enemyCount == 0 && itemcollector.points ==0 && levelCompleted != true)
            {
                anim.SetTrigger("finished");
                finishSound.Play();
                levelCompleted = true;
                Invoke("CompleteLevel", 6f);
                Debug.Log("finish");






            }*/

        }


     


   

        transform.position = Vector2.MoveTowards(transform.position, waypointTransform.position, Time.deltaTime * speed);


    }
    void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        player.GetComponent<Finishedlevel>().enemyCount--;
       
        animator.SetBool("IsDead", true);
        Debug.Log("Died");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        enemyCount -= 1;
        Enemytext.text = "Enemeis remaining: " + enemyCount;


    }
   
    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 1f, jumpableGround);
    }
}




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


