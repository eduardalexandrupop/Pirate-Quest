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
        for (int i = 0; i < endingButtons.Length; i++)
        {
            if (StoryManager.getEndingUnlocked(i))
            {
                endingButtons[i].gameObject.SetActive(true);
                endingButtons[i].GetComponentInChildren<Text>().text = StoryManager.getEndingNames()[i];
                endingButtons[i].onClick.AddListener(delegate { viewEnding(i); });
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
