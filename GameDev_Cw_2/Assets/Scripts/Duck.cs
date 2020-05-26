using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public float speed;
    public GameObject collectBarPrefab;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isAlive;
    private bool beingCollected;
    private float collectRange = 0.8f;

    private GameObject playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("changeDirection", 0f, 2f);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isAlive = true;
        beingCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("HorizontalMove", moveDirection.x);
        animator.SetFloat("VerticalMove", moveDirection.y);
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
        else
        {
            if ((playerInstance.transform.position - transform.position).magnitude < collectRange)
            {
                if (beingCollected == false)
                {
                    beingCollected = true;
                    StartCoroutine(collectDuck());
                }
            }
            else
            {
                beingCollected = false;
            }
        }
    }

    void changeDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void kill()
    {
        isAlive = false;
        animator.SetTrigger("Dead");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    public IEnumerator collectDuck()
    {
        GameObject collectBarInstance = Instantiate(collectBarPrefab, transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
        collectBarInstance.transform.localScale = new Vector3(2f, 0.2f, 1f);
        float i;
        for (i = 0f; i <= 2.1f; i += 0.2f)
        {
            if (beingCollected == true)
            {
                collectBarInstance.transform.localScale = new Vector3(2 - i, 0.2f, 1f);
                if (collectBarInstance.transform.localScale.x <= 0f)
                {
                    DucksArenaManager.huntDuck();
                    Destroy(gameObject);
                    GameObject.Destroy(collectBarInstance);
                }
                yield return new WaitForSeconds(0.07f);
            }
            else
            {
                GameObject.Destroy(collectBarInstance);
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
        {
            kill();
        }
    }

    public void setPlayerInstance(GameObject player)
    {
        playerInstance = player;
    }
}
