using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform playerPosition;

    private Vector2 attackVector;
    private float attackRange = 0.7f;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        attackVector = playerPosition.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        attackVector = playerPosition.position - transform.position;
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
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;
    }

    public bool checkIfAttacking()
    {
        return attacking;
    }
}
