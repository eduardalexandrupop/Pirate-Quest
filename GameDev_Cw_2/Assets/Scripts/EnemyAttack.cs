using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public GameObject player;
    public float attackRange;
    public string type;

    public GameObject bulletPrefab;


    private Rigidbody2D rb;
    private Vector2 attackVector;
    private bool attacking = false;

    private float meleeAttackSize;

    // blackbeard specifics
    private float blackbeardMeleeRange;
    private float blackbeardRangedRange;
    private float blackbeardMeleeAttackSize;
    private float blackbeardTransitionRange;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackVector = player.transform.position - transform.position;

        meleeAttackSize = 0.5f;

        // blackbeard specifics
        blackbeardMeleeRange = 0.7f;
        blackbeardRangedRange = 5f;
        blackbeardMeleeAttackSize = 0.6f;
        blackbeardTransitionRange = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        attackVector = (player.transform.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (gameObject.name.Contains("Blackbeard"))
        {
            float playerDistance = (player.transform.position - transform.position).magnitude;

            if (playerDistance < attackRange && attacking == false)
            {
                if (attackRange == blackbeardMeleeRange)
                    StartCoroutine(blackbeardAttackMelee(attackVector));
                else
                    StartCoroutine(blackbeardAttackRanged(attackVector));
            }

            if (playerDistance < blackbeardTransitionRange)
            {
                attackRange = blackbeardMeleeRange;
            }
            else
            {
                attackRange = blackbeardRangedRange;
            }
        }
        else if ((player.transform.position - transform.position).magnitude < attackRange && attacking == false)
        {
            if (gameObject.name.Contains("Blackbeard"))
                StartCoroutine(blackbeardAttackRanged(attackVector));
            else if (type == "melee")
                StartCoroutine(attackMelee(attackVector));
            else if (type == "ranged")
                StartCoroutine(attackRanged(attackVector));
        }
    }

    private IEnumerator attackMelee(Vector2 attackVector)
    {
        attacking = true;
        if (gameObject.GetComponent<EnemyAI>().getPushDamageEnabled())
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f);
        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector*0.5f, meleeAttackSize);
        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().loseHealth();
            }
        }
        yield return new WaitForSeconds(0.4f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;
    }

    private IEnumerator attackRanged(Vector2 attackVector)
    {
        attacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.2f);

        
        float angle = Mathf.Atan2(attackVector.y, attackVector.x) * Mathf.Rad2Deg;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(attackVector.x, attackVector.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(attackVector * 300);
        bulletInstance.GetComponent<Bullet>().origin = "enemy";

        yield return new WaitForSeconds(1.2f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;
    }

    private IEnumerator blackbeardAttackMelee(Vector2 attackVector)
    {
        attacking = true;
        if (gameObject.GetComponent<EnemyAI>().getPushDamageEnabled())
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f);
        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector*0.3f, blackbeardMeleeAttackSize);
        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().loseHealth();
                player.GetComponent<Rigidbody2D>().AddForce(attackVector * 300);
            }
        }
        yield return new WaitForSeconds(0.6f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;
    }

    private IEnumerator blackbeardAttackRanged(Vector2 attackVector)
    {
        attacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("AttackRanged");


        yield return new WaitForSeconds(0.2f);


        float angle = Mathf.Atan2(attackVector.y, attackVector.x) * Mathf.Rad2Deg;
        Vector2 bulletDirection;

        angle -= 30;
        bulletDirection = ((Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right)).normalized;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(bulletDirection.x, bulletDirection.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletDirection * 300);
        bulletInstance.GetComponent<Bullet>().origin = "enemy";


        angle += 30;
        bulletDirection = ((Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right)).normalized;
        bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(bulletDirection.x, bulletDirection.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletDirection * 300);
        bulletInstance.GetComponent<Bullet>().origin = "enemy";


        angle += 30;
        bulletDirection = ((Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right)).normalized;
        bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(bulletDirection.x, bulletDirection.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletDirection * 300);
        bulletInstance.GetComponent<Bullet>().origin = "enemy";

        yield return new WaitForSeconds(1.6f);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;
    }

    public bool checkIfAttacking()
    {
        return attacking;
    }
}
