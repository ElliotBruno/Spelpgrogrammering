using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FinishedLevel : MonoBehaviour

{
    public Animator Anim;
/*    public string Level3;
*/    
    private AudioSource FinishSound;
    private bool LevelCompleted = false;

    [SerializeField] GameObject player;
    itemcollector itemcollector;


    public int EnemyCount = 1;

    [SerializeField] private TextMeshProUGUI enemytext;

    private float dirX;
    public Animator animator;
    private SpriteRenderer sprite;
    public int Points = 8;

    [SerializeField] private TextMeshProUGUI Kiwitext;





    void Start()
    {
        FinishSound = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        itemcollector = player.GetComponent<itemcollector>();

    }



    void Update()
    {

        //Debug.Log(enemyCount);
        //Debug.Log(levelCompleted);
        //Debug.Log(points);
        if (EnemyCount == 0 && Points == 0 && LevelCompleted != true)
        {
            Anim.SetTrigger("finished");
            FinishSound.Play();
            LevelCompleted = true;
            Invoke("CompleteLevel", 1f);
            Debug.Log("finish");

        }
    }
    void CompleteLevel()
    {
        /*        SceneManager.LoadScene(Level3);
        */
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Kiwi"))
        {
            Points -= 1;
            Destroy(collision.gameObject);
            Kiwitext.text = "Fruits remaining: " + Points;
        }


    }
    void Die()
    {
        player.GetComponent<Finishedlevel>().enemyCount--;

        animator.SetBool("IsDead", true);
        Debug.Log("Died");
        GetComponent<Collider2D>().enabled = false;
        /*        this.enabled = false;
        */
        EnemyCount -= 1;
        enemytext.text = "Enemies remaining: " + EnemyCount;


    }


}

