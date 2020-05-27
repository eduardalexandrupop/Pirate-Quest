using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DucksArenaManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject crocodilePrefab;
    public GameObject duckPrefab;
    public Image attackBar;
    public Image specialAttackBar;
    public Image specialAttackImage;
    public Image[] lives;
    public Text timerText;
    public Text counterText;

    private List<Vector2> spawnPoints;
    private List<Vector2> duckSpawnPoints;
    private static GameObject playerInstance;

    private float timeBetweenWaves = 15f;
    private float arenaDuration = 60f;
    private float timer;

    private float startTime;

    private static int ducksHunted;

    private static int ducksOnMap;

    // Start is called before the first frame update
    void Start()
    {
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

        duckSpawnPoints = new List<Vector2>();
        duckSpawnPoints.Add(new Vector2(-5, -2));
        duckSpawnPoints.Add(new Vector2(-5, 2));
        duckSpawnPoints.Add(new Vector2(5, 2));
        duckSpawnPoints.Add(new Vector2(5, -2));
        duckSpawnPoints.Add(new Vector2(0, -2));
        duckSpawnPoints.Add(new Vector2(-5, 2));

        startTime = Time.time;
        timer = arenaDuration;
        InvokeRepeating("spawnEnemies", 4f, timeBetweenWaves);
        InvokeRepeating("spawnDucks", 2f, 5f);

        ducksHunted = 0;
        ducksOnMap = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        timer = arenaDuration - timePassed;
        if (timer < 0)
        {
            timer = 0;
            if (ducksHunted < 8)
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
            enemy = Instantiate(crocodilePrefab, new Vector3(spawnPoint1.x, spawnPoint1.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;
            
            enemy = Instantiate(crocodilePrefab, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;
        }
    }

    void spawnDucks()
    {
        if (ducksOnMap < 3)
        {
            Vector2 duckSpawnPoint = duckSpawnPoints[Random.Range(0, duckSpawnPoints.Count)];

            GameObject duck = Instantiate(duckPrefab, new Vector3(duckSpawnPoint.x, duckSpawnPoint.y, 0), Quaternion.identity);
            duck.GetComponent<Duck>().setPlayerInstance(playerInstance);
            ducksOnMap++;
        }
    }

    void displayTimer()
    {
        if (timerText != null)
            timerText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - (int)timer / 60).ToString("D2");
    }

    void displayCounter()
    {
        counterText.text = ducksHunted.ToString() + "/8";
    }

    public static void huntDuck()
    {
        ducksHunted++;
        ducksOnMap--;
    }

    public static GameObject getPlayerInstance()
    {
        return playerInstance;
    }
}
