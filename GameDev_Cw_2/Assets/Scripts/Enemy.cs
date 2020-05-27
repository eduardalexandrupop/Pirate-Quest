using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth(int lostAmount)
    {
        StartCoroutine(damaged());

        health -= lostAmount;
        if (health <= 0)
        {
            health = 0;
            if (!gameObject.name.Contains("Blackbeard"))
            {
                Destroy(gameObject);
            }
            if (gameObject.name.Contains("Ant"))
                AntsArenaManager.killAnt();
        }
    }

    public int getHealth()
    {
        return health;
    }

    private IEnumerator damaged()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
