using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D collider;
    public Animator animator;

    private float speed = 3.0f;
    private Vector2 moveVector;
    private bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;

        animator.SetFloat("HorizontalIdle", 0);
        animator.SetFloat("VerticalIdle", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            moveVector.x = Input.GetAxisRaw("Horizontal");
            moveVector.y = Input.GetAxisRaw("Vertical");


            animator.SetFloat("HorizontalIdle", moveVector.x);
            animator.SetFloat("VerticalIdle", moveVector.y);
        }
        else
        {
            moveVector.x = 0;
            moveVector.y = 0;
        }

        animator.SetFloat("HorizontalMove", moveVector.x);
        animator.SetFloat("VerticalMove", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }

    void FixedUpdate()
    {
        collider.enabled = false;
        RaycastHit2D collisionCheck = Physics2D.CircleCast(transform.TransformPoint(collider.offset), collider.radius, moveVector, moveVector.magnitude);
        collider.enabled = true;

        if (collisionCheck.collider == null)
            rb.MovePosition(rb.position + moveVector.normalized * speed * Time.fixedDeltaTime);
        else
            Debug.Log(collisionCheck.collider);
    }
}
