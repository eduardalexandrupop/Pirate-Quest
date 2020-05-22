using UnityEngine;
using Pathfinding;

public class PathfinderMovement : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer enemyGraphics;

    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public bool side;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    public void Flip(bool side)
    {
        bool state = (side) ? false : true;
        enemyGraphics.flipX = state;
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWayPoint >= path.vectorPath.Count)
            return;
        if (!target.gameObject.activeInHierarchy)
            gameObject.SetActive(false);

        // Eventual ai putea sa modifici variabila de forta ca sa se miste mai putin zburator asa ca e aiurea ca la pasarea din video
        // Asta daca stii, ca eu nu cred ca stiu sincer, as putea sa incerc daca vrei
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
            currentWayPoint++;

        //Aici am facut cum stiam eu sa dau flip la animatie pt x si y, daca poti sa modifici tu pt toate directiile ar fi super
        if (force.x >= 0.01f)
        {
            side = true;
            Flip(side);
        }
        else if (force.x < -0.01f)
        {
            side = false;
            Flip(side);
        }
    }
}
