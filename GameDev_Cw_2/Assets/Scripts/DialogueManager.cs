using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public Image backgroundImage;

    public Image playerImage;
    public Image otherImage;

    public Text sayer;
    public Text dialogue;

    public Button choice1Button;
    public Button choice2Button;

    public Text nextText;

    public string[] dialogueSayers;
    public string[] dialogueLines;

    public string choice1;
    public string choice2;

    public string nextSceneName;

    private int dialogueIndex;
    private bool nextAllowed;

    // Start is called before the first frame update
    void Start()
    {
        playerImage.enabled = false;
        otherImage.enabled = false;
        sayer.text = "";
        dialogue.text = "";
        choice1Button.gameObject.SetActive(false);
        choice2Button.gameObject.SetActive(false);

        choice1Button.onClick.AddListener(delegate { makeChoice(choice1Button); });
        choice2Button.onClick.AddListener(delegate { makeChoice(choice2Button); });

        nextText.enabled = false;

        dialogueIndex = 0;
        nextAllowed = false;

        StartCoroutine(waitToStart());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && nextAllowed)
        {
            dialogueIndex++;
            if (dialogueIndex == dialogueLines.Length)
            {
                if (nextSceneName.Contains("Arena"))
                    StoryManager.currentArenaSceneName = nextSceneName;
                else if (nextSceneName.Equals("WeaponSelection"))
                {
                    if(SceneManager.GetActiveScene().name.Equals("Abandon"))
                        StoryManager.currentArenaSceneName = "SkeletonsArena";
                    else
                        StoryManager.currentArenaSceneName = "SharksArena";
                }
                loadScene(nextSceneName);
            }
            else
            {
                resetDialogueUI();
                specificChanges();
                StartCoroutine(showDialogue());
            }
        }
    }

    private void resetDialogueUI()
    {
        playerImage.enabled = false;
        otherImage.enabled = false;
        sayer.text = "";
        dialogue.text = "";
        choice1Button.gameObject.SetActive(false);
        choice2Button.gameObject.SetActive(false);
        nextText.enabled = false;

        nextAllowed = false;
    }

    private void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void specificChanges()
    {
        if (dialogueLines[dialogueIndex].Contains("Blackbeard") && dialogueSayers[dialogueIndex].Equals("Crewmate"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("blackbeardBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("Patch") && dialogueSayers[dialogueIndex].Equals("Crewmate"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("shipBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("empty handed") && dialogueSayers[dialogueIndex].Equals("Narrator") && dialogueSayers[dialogueIndex+1].Equals("Patch"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("shipBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("empty handed") && dialogueSayers[dialogueIndex].Equals("Narrator") && dialogueSayers[dialogueIndex + 1].Equals("Chef"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("shipBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("empty handed") && dialogueSayers[dialogueIndex].Equals("Narrator") && dialogueSayers[dialogueIndex + 1].Equals("Captain"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("shipBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("After a few days") && dialogueSayers[dialogueIndex].Equals("Narrator"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("blackbeardShipBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("It just happens") && dialogueSayers[dialogueIndex].Equals("Blackbeard"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("abandonedIslandBackground");
        }
        else if (dialogueLines[dialogueIndex].Contains("One night") && dialogueSayers[dialogueIndex].Equals("Narrator"))
        {
            backgroundImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite("blackbeardShipBackground");
        }

    }

    private IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(showDialogue());
    }

    private IEnumerator showDialogue()
    {
        string characterName = dialogueSayers[dialogueIndex];
        if (characterName != "Narrator")
            sayer.text = characterName;

        if (characterName.Equals("You"))
        {
            playerImage.enabled = true;
            playerImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite(characterName);

            otherImage.enabled = false;
        }
        else if (characterName.Equals("Narrator"))
        {
            playerImage.enabled = false;
            otherImage.enabled = false;
        }
        else
        {
            playerImage.enabled = false;

            otherImage.enabled = true;
            otherImage.sprite = gameObject.GetComponent<SpriteManager>().getSprite(characterName);
        }


        string line = "";
        foreach (char c in dialogueLines[dialogueIndex])
        {
            line += c;
            dialogue.text = line;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.5f);

        if (dialogueIndex == dialogueLines.Length - 1 && !choice1.Equals(""))
        {

            choice1Button.GetComponentInChildren<Text>().text = choice1;
            choice2Button.GetComponentInChildren<Text>().text = choice2;
            choice1Button.gameObject.SetActive(true);
            choice2Button.gameObject.SetActive(true);
        }
        else
        {
            nextText.enabled = true;
            nextAllowed = true;
        }

    }

    private void makeChoice(Button b)
    {
        string text = b.GetComponentInChildren<Text>().text;
        if (text.Equals("Attack them"))
        {
            StoryManager.currentArenaSceneName = "AntsArena";
            loadScene("AntsArena");
        }
        else if (text.Equals("Spare them"))
        {
            dialogueSayers = new string[] { "You", "You", "Ants", "You", "Ants", "Narrator", "Patch"};
            dialogueLines = new string[] { "Don't worry! I'm not going to hurt you.",
                                           "You should move your colony as soon as possible! It's dangerous around here.",
                                           "And why should we believe you?",
                                           "My captain sent me here for your valuable antennae, and other pirates want them too. You're not safe!",
                                           "Thank you for your help, stranger! We will make up for it!",
                                           "You sail back to the ship empty handed.",
                                           "Where are the golden antennae? *squawk* Pirates, get rid of this traitor!"};

            choice1 = "";
            choice2 = "";
            dialogueIndex = 0;
            resetDialogueUI();
            StartCoroutine(showDialogue());
        }

        else if (text.Equals("Steal the honey"))
        {
            StoryManager.currentArenaSceneName = "BeesArena";
            loadScene("BeesArena");
        }
        else if (text.Equals("Leave the hive"))
        {
            dialogueSayers = new string[] { "You", "Bee", "Bee", "Narrator", "Captain Flint", "Captain Flint", "Crew member" };
            dialogueLines = new string[] { "Keep your honey. You need it more than I do...",
                                           "I appreciate that! The bees won't forget your goodwill, pirate!",
                                           "If you encounter any dangers, our soldier bees will be there for you!",
                                           "You go back to the ship empty handed.",
                                           "What is this treason? Such an important ingredient and you have nothing!?",
                                           "Crew! Cut him into pieces!",
                                           "But captain... You threw our swords overboard..."};

            choice1 = "";
            choice2 = "";
            dialogueIndex = 0;
            resetDialogueUI();
            StartCoroutine(showDialogue());
        }

        else if (text.Equals("Start hunting"))
        {
            StoryManager.currentArenaSceneName = "DucksArena";
            loadScene("DucksArena");
        }
        else if (text.Equals("Leave the lake"))
        {
            dialogueSayers = new string[] { "You", "Crocodile", "Crocodile", "Narrator", "Chef", "Chef" };
            dialogueLines = new string[] { "Fine. The lake is all yours.",
                                           "Thank you so much, stranger! Our families are very grateful for your kindness!",
                                           "If you ever find yourself in danger on the seas, we will be there!",
                                           "You sail back to the ship empty handed.",
                                           "Oh no, no, no... Please don't tell me you found no ducks at all.",
                                           "Pirates! He ate all of your food! Punish him!"};

            choice1 = "";
            choice2 = "";
            dialogueIndex = 0;
            resetDialogueUI();
            StartCoroutine(showDialogue());
        }
        else if (text.Equals("Assassinate Blackbeard"))
        {
            StoryManager.unlockEnding(3);
            loadScene("AssassinateEnding");
        }
        else if (text.Equals("Thank him for saving you"))
        {
            StoryManager.unlockEnding(2);
            loadScene("FirstMateEnding");
        }
        else if (text.Equals("Confront Blackbeard"))
        {
            StoryManager.unlockEnding(6);
            loadScene("ShotEnding");
        }
        else if (text.Equals("Sail away"))
        {
            StoryManager.unlockEnding(7);
            loadScene("EscapeEnding");
        }
    }
}
