using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public Image attackBar;

    private bool attacking;
    private float attackCooldown;
    private bool attackOnCooldown;

    private float attackSize;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        attackOnCooldown = false;
        attackCooldown = 0.8f;

        attackSize = 0.3f;
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

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector, attackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().loseHealth();
                Rigidbody2D rBod = col.gameObject.GetComponent<Rigidbody2D>();
                rBod.constraints = RigidbodyConstraints2D.None;
                rBod.constraints = RigidbodyConstraints2D.FreezeRotation;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 100);
            }
        }
        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 100);
            }
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;

        attackBar.rectTransform.sizeDelta = new Vector2(0, attackBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < attackCooldown)
        {
            attackBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / attackCooldown, attackBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(attackCooldown/100);
            timeProgress += attackCooldown / 100;
        }
        attackOnCooldown = false;
    }


    public bool checkIfAttacking()
    {
        return attacking;
    }
}
