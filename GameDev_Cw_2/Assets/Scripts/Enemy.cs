﻿using System.Collections;
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

    public void loseHealth()
    {
        health--;
        if (health == 0)
        {
            Destroy(gameObject);
            if (gameObject.name.Contains("Ant"))
                AntsArenaManager.killAnt();
        }
    }
}
