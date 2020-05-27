using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteChallenges : MonoBehaviour
{
    public Text description;

    // Start is called before the first frame update
    void Start()
    {
        description.text = StoryManager.getCompleteChallengeText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StoryManager.completeChallenge();
        }
    }
}
