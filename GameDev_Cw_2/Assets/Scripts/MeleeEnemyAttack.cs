using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    public Animator animator;
    public GameObject player;

    private Rigidbody2D rb;
    private Vector2 attackVector;
    private float attackRange = 0.7f;
    private bool attacking = false;

    private float attackSize;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackVector = player.transform.position - transform.position;

        attackSize = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        attackVector = player.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        if (attackVector.magnitude < attackRange && attacking == false)
            StartCoroutine(attack(attackVector));
    }

    private IEnumerator attack(Vector2 attackVector)
    {
        attacking = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.7f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector*0.5f, attackSize);
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

    public bool checkIfAttacking()
    {
        return attacking;
    }
}
