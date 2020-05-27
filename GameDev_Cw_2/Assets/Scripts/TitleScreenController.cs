using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public Button startButton;
    public Button endingsButton;
    public Button tutorialButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(start);
        endingsButton.onClick.AddListener(endings);
        tutorialButton.onClick.AddListener(tutorial);

        SoundManager.instance.playMenuEnding();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start()
    {
        SoundManager.instance.stopMenuEnding();
        SoundManager.instance.playCutscene();
        SceneManager.LoadScene("Intro");
    }

    private void endings()
    {
        SceneManager.LoadScene("Endings");
    }

    private void tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
