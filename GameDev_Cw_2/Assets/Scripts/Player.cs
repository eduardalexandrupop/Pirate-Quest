using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image[] lives;
    public Text dead;
    private int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        dead.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseHealth()
    {
        health--;
        foreach (Image i in lives)
            i.enabled = false;
        for (int i = 0; i < health; i++)
            lives[i].enabled = true;

        if (health == 0)
            dead.enabled = true;
    }
}
