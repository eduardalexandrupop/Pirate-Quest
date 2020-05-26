using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public bool specialAttackUnlocked;

    public Image attackBar;
    public Image specialAttackBar;

    public string attackType;

    public GameObject bulletPrefab;

    private bool attacking;
    private float attackCooldown;
    private bool attackOnCooldown;
    private float attackSize;

    private bool specialAttacking;
    private float specialAttackCooldown;
    private bool specialAttackOnCooldown;
    private float specialAttackSize;

    private List<Vector2> shootingDirections;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        attackOnCooldown = false;
        attackCooldown = 0.5f;
        attackSize = 0.4f;

        specialAttacking = false;
        specialAttackOnCooldown = false;
        specialAttackCooldown = 7f;
        specialAttackSize = 2f;

        if (attackType.Equals("boat"))
        {
            attackCooldown = 0.1f;
            specialAttackCooldown = 4f;
        }

        shootingDirections = new List<Vector2>();
        shootingDirections.Add(new Vector2(0, -1).normalized);
        shootingDirections.Add(new Vector2(-1, -1).normalized);
        shootingDirections.Add(new Vector2(-1, 0).normalized);
        shootingDirections.Add(new Vector2(-1, 1).normalized);
        shootingDirections.Add(new Vector2(0, 1).normalized);
        shootingDirections.Add(new Vector2(1, 1).normalized);
        shootingDirections.Add(new Vector2(1, 0).normalized);
        shootingDirections.Add(new Vector2(1, -1).normalized);
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
                else if (attackType.Equals("boat"))
                    StartCoroutine(attackBoat(new Vector2(0, 1)));
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(0, -1)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(0, -1)));
                else if (attackType.Equals("boat"))
                    StartCoroutine(attackBoat(new Vector2(0, -1)));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(-1, 0)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(-1, 0)));
                else if (attackType.Equals("boat"))
                    StartCoroutine(attackBoat(new Vector2(-1, 0)));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(attackMelee(new Vector2(1, 0)));
                else if (attackType.Equals("ranged"))
                    StartCoroutine(attackRanged(new Vector2(1, 0)));
                else if (attackType.Equals("boat"))
                    StartCoroutine(attackBoat(new Vector2(1, 0)));
            }
        }

        if (specialAttackOnCooldown == false && specialAttackUnlocked)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (attackType.Equals("melee"))
                    StartCoroutine(specialAttackMelee());
                else if (attackType.Equals("ranged"))
                    StartCoroutine(specialAttackRanged());
                else if (attackType.Equals("boat"))
                    StartCoroutine(specialAttackBoat());
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

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector * 0.6f, attackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                if (col.gameObject.name.Contains("Blackbeard"))
                {
                    col.gameObject.GetComponent<Enemy>().loseHealth(5);
                }
                else
                {
                    col.gameObject.GetComponent<Enemy>().loseHealth(1);
                }
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

    private IEnumerator attackBoat(Vector2 attackVector)
    {
        attacking = true;
        attackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetFloat("HorizontalAttack", attackVector.x);
        animator.SetFloat("VerticalAttack", attackVector.y);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(0.2f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position + attackVector * 0.6f, attackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
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
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 1000);
                StartCoroutine(resetEnemyCollision());
                StartCoroutine(enemyPush(col.gameObject));
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
            yield return new WaitForSeconds(attackCooldown / 100);
            timeProgress += attackCooldown / 100;
        }
        attackOnCooldown = false;
    }

    private IEnumerator specialAttackMelee()
    {
        specialAttacking = true;
        specialAttackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetTrigger("SpecialAttack");


        yield return new WaitForSeconds(0.2f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position, specialAttackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
                if (col.gameObject.name.Contains("Blackbeard"))
                {
                    col.gameObject.GetComponent<Enemy>().loseHealth(5);
                }
                else
                {
                    col.gameObject.GetComponent<Enemy>().loseHealth(1);
                }
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
                Vector2 attackVector = (col.gameObject.GetComponent<Rigidbody2D>().position - rb.position).normalized;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 400);
                StartCoroutine(resetEnemyCollision());
            }
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        specialAttacking = false;

        specialAttackBar.rectTransform.sizeDelta = new Vector2(0, specialAttackBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < specialAttackCooldown)
        {
            specialAttackBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / specialAttackCooldown, specialAttackBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(specialAttackCooldown / 100);
            timeProgress += specialAttackCooldown / 100;
        }
        specialAttackOnCooldown = false;
    }

    private IEnumerator specialAttackRanged()
    {
        specialAttacking = true;
        specialAttackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetTrigger("SpecialAttack");


        yield return new WaitForSeconds(0.2f);

        foreach (Vector2 attackVector in shootingDirections)
        {
            float angle = Mathf.Atan2(attackVector.y, attackVector.x) * Mathf.Rad2Deg;
            GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(attackVector.x, attackVector.y, 0) * 0.5f, Quaternion.AngleAxis(angle, Vector3.forward));
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(attackVector * 300);
            bulletInstance.GetComponent<Bullet>().origin = "player";

            yield return new WaitForSeconds(0.02f);
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        specialAttacking = false;

        specialAttackBar.rectTransform.sizeDelta = new Vector2(0, specialAttackBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < specialAttackCooldown)
        {
            specialAttackBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / specialAttackCooldown, specialAttackBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(specialAttackCooldown / 100);
            timeProgress += specialAttackCooldown / 100;
        }
        specialAttackOnCooldown = false;
    }

    private IEnumerator specialAttackBoat()
    {
        specialAttacking = true;
        specialAttackOnCooldown = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator.SetTrigger("SpecialAttack");


        yield return new WaitForSeconds(0.2f);

        Collider2D[] collidersAttacked = Physics2D.OverlapCircleAll(rb.position, specialAttackSize);

        foreach (Collider2D col in collidersAttacked)
        {
            if (col.gameObject.tag == "Enemy")
            {
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
                Vector2 attackVector = (col.gameObject.GetComponent<Rigidbody2D>().position - rb.position).normalized;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(attackVector * 1500);
                StartCoroutine(resetEnemyCollision());
                StartCoroutine(enemyPush(col.gameObject));
            }
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        specialAttacking = false;

        specialAttackBar.rectTransform.sizeDelta = new Vector2(0, specialAttackBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < specialAttackCooldown)
        {
            specialAttackBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / specialAttackCooldown, specialAttackBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(specialAttackCooldown / 100);
            timeProgress += specialAttackCooldown / 100;
        }
        specialAttackOnCooldown = false;
    }

    private IEnumerator resetEnemyCollision()
    {
        yield return new WaitForSeconds(1.5f);
        EnemyAI.enableEnemyCollision();
    }

    private IEnumerator enemyPush(GameObject enemy)
    {
        enemy.GetComponent<EnemyAI>().enablePushDamage();
        yield return new WaitForSeconds(1f);
        if (enemy != null)
            enemy.GetComponent<EnemyAI>().disablePushDamage();
    }

    public bool checkIfAttacking()
    {
        return (attacking && specialAttacking);
    }

    public bool getSpecialAttackUnlocked()
    {
        return specialAttackUnlocked;
    }
}
