using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;                        // GameObject the enemy will follow
    public Transform[] targets = new Transform[2];  // List of PathPoints

    private float speed;                            // Movement speed, it's higher than normal because force is used
    private bool chasing = false;                   // Checks if the enemy is chasing the target
    public float normalSpeed = 200f;                // Speed when following original path
    public float chaseSpeed = 400f;                 // Speed when chasing the target
    public float distanceToChase = 10f;             // Maximum distance to chase the target from the last PathPoint
    public float nextWaypointDistance = 3f;         // Distance to next way point required to go to next one

    private int currentWayPoint = 0;                // Current way point from the path
    private int currentTarget = 0;                  // Current PathPoint to go to

    private Path path;                                      // Path component from Pathfinding
    private Seeker seeker;                                  // gameObject component that checks current path status
    //private Animator animator;                              // Animator component for animations
    private Rigidbody2D rigidbody2D;                        // gameObject component that manages physics

    // Start is called before the first frame update
    void Start()
    {
        speed = normalSpeed;
        seeker = GetComponent<Seeker>();
        //animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Calls the UpdatePath function, starting 0 seconds after running, and calling it each 0.5 seconds
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            // Path not found
            return;
        
        if(currentWayPoint >= path.vectorPath.Count)
            // Reached end of path
            return;

        // Traces the direction to the next wayPoint and adds force to move the gameObject
        Vector2 distance = (Vector2)path.vectorPath[currentWayPoint];
        Vector2 direction = (distance - rigidbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rigidbody2D.AddForce(force);

        // Checks if the current distance to the next way point is closer than the nextWaypointDistance, and if so, searches for the next wayPoint
        float wayPointDistance = Vector2.Distance(rigidbody2D.position, distance);
        if(wayPointDistance < nextWaypointDistance)
            currentWayPoint++;

        // If chasing, check for max distance to chase, if exceeded, return to original path
        float targetDistance = Vector2.Distance(rigidbody2D.position, 
            targets[currentTarget].position);
        if(chasing && targetDistance >= distanceToChase)
        {
            chasing = false;
            speed = normalSpeed;
        }

        /* 
        // Animation for running animation
        transform.GetChild(0).GetComponent<Animator>()
            .SetFloat("velocity", Mathf.Abs(ridigbody2D.velocity.x 
                                              + ridigbody2D.velocity.y));

        // Flip if horizontal direction changes
        if (ridigbody2D.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (ridigbody2D.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PathPoint"))
        {
            if (currentTarget >= targets.Length - 1)
            {
                currentTarget = 0;
            }
            else
            {
                currentTarget++;
            }
        }
        else if(other.CompareTag("Player"))
        {
            chasing = true;
            speed = chaseSpeed;
        }

    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            if(!chasing)
            {
                seeker.StartPath(rigidbody2D.position, 
                    targets[currentTarget].position, OnPathComplete);
            }
            else
            {
                seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
        else
        {
            currentTarget = 0;
        }
    }
}
