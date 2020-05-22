﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeAI : MonoBehaviour
{

    public Transform target;
    public float speed;
    public float nextWaypointDistance;

    public Animator animator;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAttacking())
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;

        if (!isAttacking())
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    void Update()
    {
        if (direction.x != 0 && direction.y != 0)
        {
            animator.SetFloat("HorizontalIdle", direction.x);
            animator.SetFloat("VerticalIdle", direction.y);
            animator.SetFloat("HorizontalMove", direction.x);
            animator.SetFloat("VerticalMove", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);
        }

        if (isAttacking() == true)
            animator.SetFloat("Speed", 0);
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

    private bool isAttacking()
    {
        return gameObject.GetComponent<MeleeEnemyAttack>().checkIfAttacking();
    }
}
