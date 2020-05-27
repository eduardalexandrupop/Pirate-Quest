
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource menuEnding;
    public AudioSource cutScene;
    public AudioSource arenas;


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

    public void playMenuEnding()
    {
        stopArenas();
        stopCutscene();
        if (menuEnding.isPlaying == false)
            menuEnding.Play();
    }

    public void stopMenuEnding()
    {
        menuEnding.Stop();
    }

    public void playCutscene()
    {
        stopMenuEnding();
        stopArenas();
        if (cutScene.isPlaying == false)
            cutScene.Play();
    }

    public void stopCutscene()
    {
        cutScene.Stop();
    }

    public void playArenas()
    {
        stopCutscene();
        stopMenuEnding();
        if (arenas.isPlaying == false)
            arenas.Play();
    }

    public void stopArenas()
    {
        arenas.Stop();
    }
}
