using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    private static bool[] endingsUnlocked = new bool[8] { false, false, false, false, false, false, false, false};
    private static string[] endingNames = 
        new string[8] { "The replacement", "The courageous failure", "The helping hand", "The traitor", "The pickpocket", "The bad thief", "The fool's bravery", "The runaway" };

    public static string currentArenaSceneName;

    public static int captainChallengesCompleted;

    public static string selectedWeapon;

    public static bool unlockedSwordSpecial;
    public static bool unlockedBoatSpecial;
    public static bool unlockedGunSpecial;
    public static bool unlockedAnts;
    public static bool unlockedCrocodiles;
    public static bool unlockedBees;

    // Start is called before the first frame update
    void Start()
    {
        currentArenaSceneName = null;
        captainChallengesCompleted = 0;
        selectedWeapon = null;

        unlockedSwordSpecial = false;
        unlockedBoatSpecial = false;
        unlockedGunSpecial = false;
        unlockedAnts = false;
        unlockedCrocodiles = false;
        unlockedBees = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool getEndingUnlocked(int i)
    {
        return endingsUnlocked[i];
    }

    public static void unlockEnding(int i)
    {
        endingsUnlocked[i] = true;
    }

    public static string[] getEndingNames()
    {
        return endingNames;
    }

    public static void failChallenge()
    {
        if (currentArenaSceneName.Contains("Pirate") || currentArenaSceneName.Contains("Ant") || currentArenaSceneName.Contains("Duck") || currentArenaSceneName.Contains("Bee"))
            SceneManager.LoadScene("TryAgain");
        else if (currentArenaSceneName.Contains("Skeleton"))
        {
            SceneManager.LoadScene("FailedSkeletons");
        }
        else if (currentArenaSceneName.Contains("Shark"))
        {
            SceneManager.LoadScene("FailedSharks");
        }
        else if (currentArenaSceneName.Contains("Blackbeard"))
        {
            if (captainChallengesCompleted >= 2)
            {
                unlockEnding(5);
                SceneManager.LoadScene("SlavesEnding");
            }
            else
            {
                unlockEnding(1);
                SceneManager.LoadScene("ExecutedEnding");
            }
        }
    }

    public static void completeChallenge()
    {
        if (currentArenaSceneName.Equals("MeleePirateArena"))
        {
            unlockedAnts = true;
            SceneManager.LoadScene("Monkey");
        }
        else if (currentArenaSceneName.Contains("Ant"))
        {
            unlockedSwordSpecial = true;
            captainChallengesCompleted++;
            SceneManager.LoadScene("Monkey");
        }
        else if (currentArenaSceneName.Equals("RangedPirateArena"))
        {
            unlockedCrocodiles = true;
            SceneManager.LoadScene("Captain");
        }
        else if (currentArenaSceneName.Contains("Duck"))
        {
            unlockedGunSpecial = true;
            captainChallengesCompleted++;
            SceneManager.LoadScene("Captain");
        }
        else if (currentArenaSceneName.Equals("BoatPirateArena"))
        {
            unlockedBees = true;
            if (captainChallengesCompleted >= 2)
                SceneManager.LoadScene("TimeSkip");
            else
                SceneManager.LoadScene("Abandon");
        }
        else if (currentArenaSceneName.Contains("Bee"))
        {
            unlockedBoatSpecial = true;
            captainChallengesCompleted++;
            if (captainChallengesCompleted >= 2)
                SceneManager.LoadScene("TimeSkip");
            else
                SceneManager.LoadScene("Abandon");
        }
        else if (currentArenaSceneName.Contains("Skeleton"))
        {
            SceneManager.LoadScene("FoundTreasure");
        }
        else if (currentArenaSceneName.Contains("Shark"))
        {
            SceneManager.LoadScene("FoundKey");
        }
        else if (currentArenaSceneName.Contains("Blackbeard"))
        {
            if (captainChallengesCompleted >= 2)
            {
                unlockEnding(4);
                SceneManager.LoadScene("StealTreasureEnding");
            }
            else
            {
                unlockEnding(0);
                SceneManager.LoadScene("NewBlackbeardEnding");
            }
        }
    }

    public static string getCompleteChallengeText()
    {
        if (currentArenaSceneName.Equals("MeleePirateArena"))
        {
            return "You survive the pirates on the ship.";
        }
        else if (currentArenaSceneName.Contains("Ant"))
        {
            return "You bring the antennae to the crew and learn some new sword tricks.";
        }
        else if (currentArenaSceneName.Equals("RangedPirateArena"))
        {
            return "You survive the pirates on the ship.";
        }
        else if (currentArenaSceneName.Contains("Duck"))
        {
            return "You bring the duck meat to the crew and learn some new gun tricks.";
        }
        else if (currentArenaSceneName.Equals("BoatPirateArena"))
        {
            return "You survive the pirates on the ship.";
        }
        else if (currentArenaSceneName.Contains("Bee"))
        {
            return "You bring the gathered honey to the captain and learn some new boat tricks.";
        }
        else if (currentArenaSceneName.Contains("Skeleton"))
        {
            return "You found the hidden treasure.";
        }
        else if (currentArenaSceneName.Contains("Shark"))
        {
            return "You found the key for the treasure chest.";
        }
        else if (currentArenaSceneName.Contains("Blackbeard"))
        {
                return "You defeated Captain Blackbeard.";
        }
        else return null;
    }

    public static bool getUnlockedSpecial(string weapon)
    {
        if (weapon.Equals("sword"))
            return unlockedSwordSpecial;
        else if (weapon.Equals("gun"))
            return unlockedGunSpecial;
        else if (weapon.Equals("boat"))
            return unlockedBoatSpecial;
        else
            return false;
    }

}
