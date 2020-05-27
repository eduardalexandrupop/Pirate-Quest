using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingShower : MonoBehaviour
{
    public Button[] endingButtons;

    public Button backButton;

    public string[] endingSceneNames;

    // Start is called before the first frame update
    void Start()
    {
        endingButtons[0].onClick.AddListener(delegate { viewEnding(0); });
        endingButtons[1].onClick.AddListener(delegate { viewEnding(1); });
        endingButtons[2].onClick.AddListener(delegate { viewEnding(2); });
        endingButtons[3].onClick.AddListener(delegate { viewEnding(3); });
        endingButtons[4].onClick.AddListener(delegate { viewEnding(4); });
        endingButtons[5].onClick.AddListener(delegate { viewEnding(5); });
        endingButtons[6].onClick.AddListener(delegate { viewEnding(6); });
        endingButtons[7].onClick.AddListener(delegate { viewEnding(7); });
        for (int i = 0; i < endingButtons.Length; i++)
        {
            if (StoryManager.getEndingUnlocked(i))
            {
                endingButtons[i].gameObject.SetActive(true);
                endingButtons[i].GetComponentInChildren<Text>().text = StoryManager.getEndingNames()[i];
            }
            else
            {
                endingButtons[i].gameObject.SetActive(false);
            }
        }

        backButton.onClick.AddListener(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void back()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    void viewEnding(int i)
    {
        SceneManager.LoadScene(endingSceneNames[i]);
    }
}
