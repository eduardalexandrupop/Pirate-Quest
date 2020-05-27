using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackBut : MonoBehaviour
{
    public Button backbut;

    // Start is called before the first frame update
    void Start()
    {
        backbut.onClick.AddListener(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void back()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
