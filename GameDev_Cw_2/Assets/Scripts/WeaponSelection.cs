using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponSelection : MonoBehaviour
{
    public Button sword;
    public Button gun;
    public Button boat;

    // Start is called before the first frame update
    void Start()
    {
        sword.onClick.AddListener(selectSword);
        gun.onClick.AddListener(selectGun);
        boat.onClick.AddListener(selectBoat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void selectSword()
    {
        StoryManager.selectedWeapon = "sword";
        SceneManager.LoadScene(StoryManager.currentArenaSceneName);
    }

    void selectGun()
    {
        StoryManager.selectedWeapon = "gun";
        SceneManager.LoadScene(StoryManager.currentArenaSceneName);
    }

    void selectBoat()
    {
        StoryManager.selectedWeapon = "boat";
        SceneManager.LoadScene(StoryManager.currentArenaSceneName);
    }
}
