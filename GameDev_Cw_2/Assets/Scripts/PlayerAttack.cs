using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    private bool attacking;
    private float attackCooldown;
    private bool attackOnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        attackOnCooldown = false;
        attackCooldown = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackOnCooldown == false)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(attack(new Vector2(0, 1)));
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(attack(new Vector2(0, -1)));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(attack(new Vector2(-1, 0)));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(attack(new Vector2(1, 0)));
            }
        }
    }

    private IEnumerator attack(Vector2 attackVector)
    {
        attacking = true;
        attackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;

        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }


    public bool checkIfAttacking()
    {
        return attacking;
    }
}
