using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public Button startButton;
    public Button endingsButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(start);
        endingsButton.onClick.AddListener(endings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void start()
    {
        SceneManager.LoadScene("Intro");
    }

    private void endings()
    {
        SceneManager.LoadScene("Endings");
    }
}
