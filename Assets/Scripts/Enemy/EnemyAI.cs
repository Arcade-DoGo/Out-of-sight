using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200.0f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reachEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            return;
        
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            return;
        }
        else
        {
            reachEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rigidbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidbody2D.AddForce(force);

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
