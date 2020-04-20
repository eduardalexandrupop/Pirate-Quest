using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D collider;
    public Animator animator;
    public Transform playerPosition;

    private float speed = 1.5f;
    private Vector2 moveVector;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = (playerPosition.position - transform.position).normalized;
        animator.SetFloat("HorizontalIdle", moveVector.x);
        animator.SetFloat("VerticalIdle", moveVector.y);
        animator.SetFloat("HorizontalMove", moveVector.x);
        animator.SetFloat("VerticalMove", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);

        if (isAttacking() == true)
            animator.SetFloat("Speed", 0);
    }

    void FixedUpdate()
    {
       if (isAttacking() == false)
            rb.MovePosition(rb.position + moveVector.normalized * speed * Time.fixedDeltaTime);
    }


    private bool isAttacking()
    {
        return gameObject.GetComponent<MeleeEnemyAttack>().checkIfAttacking();
    }

}
