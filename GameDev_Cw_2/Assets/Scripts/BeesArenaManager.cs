using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeesArenaManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject beePrefab;
    public GameObject honeyPrefab;
    public Image attackBar;
    public Image specialAttackBar;
    public Image specialAttackImage;
    public Image[] lives;
    public Text timerText;
    public Text counterText;

    private List<Vector2> spawnPoints;
    private List<Vector2> honeySpawnPoints;
    private static GameObject playerInstance;

    private float timeBetweenWaves = 7f;
    private float arenaDuration = 60f;
    private float timer;

    private float startTime;

    private static int honeyGathered;

    private static int honeyOnMap;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.playArenas();
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerInstance.GetComponent<PlayerAttack>().attackBar = attackBar;
        playerInstance.GetComponent<Player>().lives = lives;

        playerInstance.GetComponent<PlayerAttack>().specialAttackBar = specialAttackBar;
        if (playerInstance.GetComponent<PlayerAttack>().getSpecialAttackUnlocked() == false)
        {
            specialAttackBar.gameObject.SetActive(false);
            specialAttackImage.gameObject.SetActive(false);
        }

        spawnPoints = new List<Vector2>();
        spawnPoints.Add(new Vector2(8, 3));
        spawnPoints.Add(new Vector2(-8, -3));
        spawnPoints.Add(new Vector2(8, -3));
        spawnPoints.Add(new Vector2(-8, 3));
        spawnPoints.Add(new Vector2(6, 5));
        spawnPoints.Add(new Vector2(-6, -5));
        spawnPoints.Add(new Vector2(6, -5));
        spawnPoints.Add(new Vector2(-6, 5));

        honeySpawnPoints = new List<Vector2>();
        honeySpawnPoints.Add(new Vector2(-5, -2));
        honeySpawnPoints.Add(new Vector2(-5, 2));
        honeySpawnPoints.Add(new Vector2(5, 2));
        honeySpawnPoints.Add(new Vector2(5, -2));
        honeySpawnPoints.Add(new Vector2(0, -2));
        honeySpawnPoints.Add(new Vector2(-5, 2));

        startTime = Time.time;
        timer = arenaDuration;
        InvokeRepeating("spawnEnemies", 4f, timeBetweenWaves);
        InvokeRepeating("spawnHoney", 2f, 5f);

        honeyGathered = 0;
        honeyOnMap = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        timer = arenaDuration - timePassed;
        if (timer < 0)
        {
            timer = 0;
            if (honeyGathered < 8)
                StoryManager.failChallenge();
            else
                SceneManager.LoadScene("CompleteChallenge");
        }

        displayTimer();
        displayCounter();
    }

    void spawnEnemies()
    {
        if (timer > 0)
        {
            Vector2 spawnPoint1 = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector2 spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Count)];
            while (spawnPoint2 == spawnPoint1)
                spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject enemy;
            enemy = Instantiate(beePrefab, new Vector3(spawnPoint1.x, spawnPoint1.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;

            enemy = Instantiate(beePrefab, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;
        }
    }

    void spawnHoney()
    {
        if (honeyOnMap < 3)
        {
            Vector2 honeySpawnPoint = honeySpawnPoints[Random.Range(0, honeySpawnPoints.Count)];

            GameObject honey = Instantiate(honeyPrefab, new Vector3(honeySpawnPoint.x, honeySpawnPoint.y, 0), Quaternion.identity);
            honey.GetComponent<HoneyComb>().setPlayerInstance(playerInstance);
            honeyOnMap++;
        }
    }

    void displayTimer()
    {
        if (timerText != null)
            timerText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - (int)timer / 60).ToString("D2");
    }

    void displayCounter()
    {
        counterText.text = honeyGathered.ToString() + "/8";
    }

    public static void gatherHoney()
    {
        honeyGathered++;
        honeyOnMap--;
    }

    public static GameObject getPlayerInstance()
    {
        return playerInstance;
    }
}
