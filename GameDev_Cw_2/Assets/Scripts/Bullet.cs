using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string origin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (origin == "player")
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            else
            {
                if (collision.gameObject.tag.Equals("Enemy"))
                {
                    collision.gameObject.GetComponent<Enemy>().loseHealth();
                }
                Destroy(gameObject);
            }
        }
        else if (origin == "enemy")
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                Destroy(gameObject);
            }
            else
            {
                if (collision.gameObject.tag.Equals("Player"))
                {
                    collision.gameObject.GetComponent<Player>().loseHealth();
                }
                Destroy(gameObject);
            }
        }
    }
}
