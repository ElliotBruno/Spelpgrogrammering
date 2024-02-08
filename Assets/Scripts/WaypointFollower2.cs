using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower2 : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;


    private Transform waypointTransform;
    private int currentWaypointIndex = 0;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        Debug.Log(currentWaypointIndex);
        waypointTransform = waypoints[currentWaypointIndex].transform;


        if (Vector2.Distance(waypointTransform.position, transform.position) < .1f)
        {
            anim.SetBool("Hit", true);

            Debug.Log(currentWaypointIndex);
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }




        transform.position = Vector2.MoveTowards(transform.position, waypointTransform.position, Time.deltaTime * speed);
    }

    public void ResetAnim()
    {
        anim.SetBool("Hit", false);
    }
}

