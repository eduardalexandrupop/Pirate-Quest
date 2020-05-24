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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackVector = player.transform.position - transform.position;

        meleeAttackSize = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        attackVector = (player.transform.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if ((player.transform.position - transform.position).magnitude < attackRange && attacking == false)
        {
            if (type == "melee")
                StartCoroutine(attackMelee(attackVector));
            else if (type == "ranged")
                StartCoroutine(attackRanged(attackVector));
        }
    }

    private IEnumerator attackMelee(Vector2 attackVector)
    {
        attacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.7f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector*0.5f, meleeAttackSize);
        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Player")
            {
                player.GetComponent<Player>().loseHealth();
            }
        }

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
    public bool checkIfAttacking()
    {
        return attacking;
    }
}
