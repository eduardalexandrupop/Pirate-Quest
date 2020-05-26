using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackbeardArenaManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject blackbeardPrefab;
    public GameObject meleePiratePrefab;
    public GameObject rangedPiratePrefab;
    public GameObject boatPiratePrefab;

    public Image attackBar;
    public Image specialAttackBar;
    public Image specialAttackImage;
    public Image[] lives;
    public Text dead;

    private List<Vector2> spawnPoints;
    private Vector2 blackbeardSpawnPoint;

    private static GameObject playerInstance;
    private static GameObject blackbeardInstance;

    private List<GameObject> enemies;

    private float timeBetweenWaves = 10f;

    // bee power
    public bool beeUnlocked;
    public Image beeBar;
    public Image beeImage;
    public GameObject friendlyBeePrefab;

    private Vector2 friendlyBeeSpawnPoint;
    private float beeCooldown;
    private bool beeOnCooldown;

    private static List<GameObject> friendlyBees;

    public Image blackbeardHPbar;

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(-5, 0, 0), Quaternion.identity);
        playerInstance.GetComponent<PlayerAttack>().attackBar = attackBar;
        playerInstance.GetComponent<Player>().lives = lives;
        playerInstance.GetComponent<Player>().dead = dead;

        playerInstance.GetComponent<PlayerAttack>().specialAttackBar = specialAttackBar;
        if (playerInstance.GetComponent<PlayerAttack>().getSpecialAttackUnlocked() == false)
        {
            specialAttackBar.gameObject.SetActive(false);
            specialAttackImage.gameObject.SetActive(false);
        }

        if (beeUnlocked == false)
        {
            beeBar.gameObject.SetActive(false);
            beeImage.gameObject.SetActive(false);
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

        enemies = new List<GameObject>();

        blackbeardSpawnPoint = new Vector2(5, 0);
        spawnBlackbeard();

        InvokeRepeating("spawnEnemies", 4f, timeBetweenWaves);

        // bee power
        friendlyBeeSpawnPoint = new Vector2(0, 5);
        beeCooldown = 15f;
        beeOnCooldown = false;

        friendlyBees = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // bee power
        if (beeOnCooldown == false && beeUnlocked)
        {
            if (Input.GetKey(KeyCode.R))
            {
                StartCoroutine(beePower());
            }
        }

        updateBlackbeardHPUI();
    }

    void FixedUpdate()
    {
        foreach (GameObject bee in friendlyBees)
        {
            if (bee != null)
                bee.GetComponent<EnemyAI>().target = blackbeardInstance;
        }
    }

    void spawnBlackbeard()
    {
        blackbeardInstance = Instantiate(blackbeardPrefab, new Vector3(blackbeardSpawnPoint.x, blackbeardSpawnPoint.y, 0), Quaternion.identity);
        blackbeardInstance.GetComponent<EnemyAI>().target = playerInstance;
        blackbeardInstance.GetComponent<EnemyAttack>().player = playerInstance;

        blackbeardHPbar.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 60);
        blackbeardHPbar.GetComponent<RectTransform>().localPosition = new Vector3(-35, 0, 0);
    }

    public void updateBlackbeardHPUI()
    {
        int newBarLength = 280 * blackbeardInstance.GetComponent<Enemy>().getHealth() / 100;

        blackbeardHPbar.GetComponent<RectTransform>().sizeDelta = new Vector2(newBarLength, 60);
        blackbeardHPbar.GetComponent<RectTransform>().localPosition = new Vector3(-35 + (280 - newBarLength) / 2, 0, 0);
    }

    void spawnEnemies()
    {
        if (blackbeardInstance.GetComponent<Enemy>().getHealth() <= 60 && countEnemies() < 2)
        {
            Vector2 spawnPoint1 = spawnPoints[Random.Range(0, spawnPoints.Count)];
            Vector2 spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Count)];
            while (spawnPoint2 == spawnPoint1)
                spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject enemy;

            GameObject enemyType;
            float seed = Random.Range(0.0f, 1.0f);
            if (seed <= 0.33f)
                enemyType = meleePiratePrefab;
            else if (seed <= 0.66f)
                enemyType = rangedPiratePrefab;
            else
                enemyType = boatPiratePrefab;

            enemy = Instantiate(enemyType, new Vector3(spawnPoint1.x, spawnPoint1.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;

            enemies.Add(enemy);

            seed = Random.Range(0.0f, 1.0f);
            if (seed <= 0.33f)
                enemyType = meleePiratePrefab;
            else if (seed <= 0.66f)
                enemyType = rangedPiratePrefab;
            else
                enemyType = boatPiratePrefab;

            enemy = Instantiate(enemyType, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;

            enemies.Add(enemy);
        }
    }

    private int countEnemies()
    {
        int enemiesNo = 0;
        foreach (GameObject enemy in enemies)
            if (enemy != null)
                enemiesNo++;

        return enemiesNo;
    }

    public static GameObject getPlayerInstance()
    {
        return playerInstance;
    }

    private IEnumerator beePower()
    {
        beeOnCooldown = true;
        GameObject friendlyBee = Instantiate(friendlyBeePrefab, new Vector3(friendlyBeeSpawnPoint.x, friendlyBeeSpawnPoint.y, 0f), Quaternion.identity);
        friendlyBees.Add(friendlyBee);

        beeBar.rectTransform.sizeDelta = new Vector2(0, beeBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < beeCooldown)
        {
            beeBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / beeCooldown, beeBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(beeCooldown / 100);
            timeProgress += beeCooldown / 100;
        }
        beeOnCooldown = false;
    }


    public static List<GameObject> getFriendlyBees()
    {
        return friendlyBees;
    }
}
