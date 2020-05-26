using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public GameObject target;
    public float speed;
    public float nextWaypointDistance;

    public Animator animator;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    private Vector2 direction;

    private static bool enemyCollision = true;
    private bool pushDamage = false;

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

        if (!isAttacking() && pushDamage == false)
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
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
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
        if (gameObject.GetComponent<EnemyAttack>() != null)
            return gameObject.GetComponent<EnemyAttack>().checkIfAttacking();
        else
            return false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (collision.gameObject.tag == "Enemy" && enemyCollision == false)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (collision.gameObject.tag == "Enemy" && enemyCollision == false)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (collision.gameObject.tag == "Obstacle" && pushDamage == true)
        {
            gameObject.GetComponent<Enemy>().loseHealth();
        }
    }

    public static void disableEnemyCollision()
    {
        enemyCollision = false;
    }

    public static void enableEnemyCollision()
    {
        enemyCollision = true;
    }

    public void enablePushDamage()
    {
        pushDamage = true;
    }

    public void disablePushDamage()
    {
        pushDamage = false;
    }

    public bool getPushDamageEnabled()
    {
        return pushDamage;
    }
}
