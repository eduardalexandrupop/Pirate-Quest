using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private static bool[] endingsUnlocked = new bool[8] { false, false, false, false, false, false, false, false};
    private static string[] endingNames = 
        new string[8] { "The replacement", "The courageous failure", "The helping hand", "The traitor", "The pickpocket", "The bad thief", "The fool's bravery", "The runaway" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool getEndingUnlocked(int i)
    {
        return endingsUnlocked[i];
    }

    public static string[] getEndingNames()
    {
        return endingNames;
    }
}
