using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image[] lives;
    private int health = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth()
    {
        StartCoroutine(damaged());
        health--;
        foreach (Image i in lives)
            i.enabled = false;
        for (int i = 0; i < health; i++)
            lives[i].enabled = true;

        if (health <= 0)
        {
            StoryManager.failChallenge();
        }
    }

    private IEnumerator damaged()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255,0,0);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
}
