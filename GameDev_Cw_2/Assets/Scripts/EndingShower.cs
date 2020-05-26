using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingShower : MonoBehaviour
{
    public Button[] endingButtons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < endingButtons.Length; i++)
        {
            if (StoryManager.getEndingUnlocked(i))
            {
                endingButtons[i].enabled = true;
               // endingButtons[i].visible = true;
                endingButtons[i].GetComponent<Text>().text = StoryManager.getEndingNames()[i];
            }
            else
            {
                endingButtons[i].enabled = false;
               // endingButtons[i].visible = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
