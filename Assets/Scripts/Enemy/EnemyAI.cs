using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;                // GameObject the enemy will follow

    public float speed = 200.0f;            // Movement speed, it's higher than normal because force is used
    public float nextWaypointDistance = 3f; // Distance to next way point required to go to next one

    Path path;                              // Path object from Pathfinding
    int currentWayPoint = 0;                // Current way point from the path

    Seeker seeker;                          // gameObject component that checks current path status
    Rigidbody2D rigidbody2D;                // gameObject component that manages physics

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
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
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rigidbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rigidbody2D.AddForce(force);

        // Checks if the current distance to the next way point is closer than the nextWaypointDistance, and if so, updates the path
        float distance = Vector2.Distance(rigidbody2D.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWaypointDistance)
            currentWayPoint++;
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
}
