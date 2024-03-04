using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Death : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float healthAmount = 100;
    // Start is called before the first frame update

    [SerializeField] private AudioSource deathSoundEffect;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
      
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Spikes")) || (collision.gameObject.CompareTag("SpikeMan")) || (collision.gameObject.CompareTag("Bottom")) || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(collision.gameObject.name);
            Die();

        }

    }
    
    private void Die()
    {
        if (healthAmount<0)
        {
            deathSoundEffect.Play();
            anim.SetTrigger("death");
            rb.bodyType = RigidbodyType2D.Static;
        }
        


    }

    private void RestartLive()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
     

    

