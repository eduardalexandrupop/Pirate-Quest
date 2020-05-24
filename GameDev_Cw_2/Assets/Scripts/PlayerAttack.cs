using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public Image attackBar;

    public string attackType;

    public GameObject bulletPrefab;

    private bool attacking;
    private float attackCooldown;
    private bool attackOnCooldown;

    private float attackSize;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        attackOnCooldown = false;
        attackCooldown = 0.5f;

        attackSize = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackOnCooldown == false)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(0, 1)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(0, 1)));
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(0, -1)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(0, -1)));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(-1, 0)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(-1, 0)));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(1, 0)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(1, 0)));
            }
        }
    }

    private IEnumerator attackMelee(Vector2 attackVector)
    {
        attacking = true;
        attackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.2f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector * 0.5f, attackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().loseHealth();
                Rigidbody2D rBod = col.gameObject.GetComponent<Rigidbody2D>();
                rBod.constraints = RigidbodyConstraints2D.None;
                rBod.constraints = RigidbodyConstraints2D.FreezeRotation;
                EnemyAI.disableEnemyCollision();
            }
        }
        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 250);
                StartCoroutine(resetEnemyCollision());
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

    
    private IEnumerator attackRanged(Vector2 attackVector)
    {
        attacking = true;
        attackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.2f);

        float angle = Mathf.Atan2(attackVector.y, attackVector.x) * Mathf.Rad2Deg;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(attackVector.x, attackVector.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(attackVector * 300);
        bulletInstance.GetComponent<Bullet>().origin = "player";

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        attacking = false;

        attackBar.rectTransform.sizeDelta = new Vector2(0, attackBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < attackCooldown)
        {
            attackBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / attackCooldown, attackBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(attackCooldown / 100);
            timeProgress += attackCooldown / 100;
        }
        attackOnCooldown = false;
    }

    private IEnumerator resetEnemyCollision()
    {
        yield return new WaitForSeconds(1);
        EnemyAI.enableEnemyCollision();
    }

    public bool checkIfAttacking()
    {
        return attacking;
    }
}
