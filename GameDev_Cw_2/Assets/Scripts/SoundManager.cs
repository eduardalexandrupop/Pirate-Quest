
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource buttonClick;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void playButtonClick()
    {
        buttonClick.Play();
    }

    public void playMenuTrack()
    {
        if (menuTrack.isPlaying == false)
            menuTrack.Play();
    }


}*/
