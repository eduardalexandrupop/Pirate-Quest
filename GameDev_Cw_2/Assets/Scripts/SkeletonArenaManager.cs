using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonArenaManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject skeletonPrefab;
    public GameObject sandPrefab;
    public Image attackBar;
    public Image specialAttackBar;
    public Image specialAttackImage;
    public Image[] lives;
    public Text dead;
    public Text timerText;

    private List<Vector2> spawnPoints;
    private List<Vector2> sandSpawnPoints;

    private List<GameObject> sands;


    private static GameObject playerInstance;

    private float timeBetweenWaves = 5f;
    private float arenaDuration = 60f;
    private float timer;

    private float startTime;

    private static int sandChecked;

    private static int sandOnMap;

    private static bool foundChest;

    // ant power
    public bool antUnlocked;
    public Image antBar;
    public Image antImage;
    public GameObject friendlyAntPrefab;

    private Vector2 friendlyAntSpawnPoint;
    private float antCooldown;
    private bool antOnCooldown;

    private static List<GameObject> friendlyAnts;

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerInstance.GetComponent<PlayerAttack>().attackBar = attackBar;
        playerInstance.GetComponent<Player>().lives = lives;
        playerInstance.GetComponent<Player>().dead = dead;

        playerInstance.GetComponent<PlayerAttack>().specialAttackBar = specialAttackBar;
        if (playerInstance.GetComponent<PlayerAttack>().getSpecialAttackUnlocked() == false)
        {
            specialAttackBar.gameObject.SetActive(false);
            specialAttackImage.gameObject.SetActive(false);
        }

        if (antUnlocked == false)
        {
            antBar.gameObject.SetActive(false);
            antImage.gameObject.SetActive(false);
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

        sandSpawnPoints = new List<Vector2>();
        sandSpawnPoints.Add(new Vector2(-5, -2));
        sandSpawnPoints.Add(new Vector2(-5, 2));
        sandSpawnPoints.Add(new Vector2(5, 2));
        sandSpawnPoints.Add(new Vector2(5, -2));
        sandSpawnPoints.Add(new Vector2(0, -2));
        sandSpawnPoints.Add(new Vector2(-5, 2));

        sands = new List<GameObject>();

        startTime = Time.time;
        timer = arenaDuration;
        InvokeRepeating("spawnEnemies", 4f, timeBetweenWaves);
        InvokeRepeating("spawnSand", 2f, 5f);

        sandChecked = 0;
        sandOnMap = 0;

        foundChest = false;

        // ant power
        friendlyAntSpawnPoint = new Vector2(0, 5);
        antCooldown = 15f;
        antOnCooldown = false;

        friendlyAnts = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        timer = arenaDuration - timePassed;
        if (timer < 0)
            timer = 0;

        displayTimer();


        // ant power
        if (antOnCooldown == false && antUnlocked)
        {
            if (Input.GetKey(KeyCode.R))
            {
                StartCoroutine(antPower());
            }
        }
    }

    void FixedUpdate()
    {
        foreach (GameObject ant in friendlyAnts)
        {
            if (ant != null)
                ant.GetComponent<EnemyAI>().target = getClosestSand(ant);
        }
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
            enemy = Instantiate(skeletonPrefab, new Vector3(spawnPoint1.x, spawnPoint1.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;

            enemy = Instantiate(skeletonPrefab, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;
        }
    }

    void spawnSand()
    {
        if (sandOnMap < 3 && foundChest == false)
        {
            Vector2 sandSpawnPoint = sandSpawnPoints[Random.Range(0, sandSpawnPoints.Count)];

            GameObject sand = Instantiate(sandPrefab, new Vector3(sandSpawnPoint.x, sandSpawnPoint.y, 0), Quaternion.identity);
            sand.GetComponent<Sand>().setPlayerInstance(playerInstance);
            sandOnMap++;

            sands.Add(sand);
        }
    }

    void displayTimer()
    {
        if (timerText != null)
            timerText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - (int)timer / 60).ToString("D2");
    }

    public static void checkSand()
    {
        sandChecked++;
        sandOnMap--;

        if (sandChecked == 7)
            foundChest = true;
    }

    public static GameObject getPlayerInstance()
    {
        return playerInstance;
    }

    public static int getSandChecked()
    {
        return sandChecked;
    }

    private IEnumerator antPower()
    {
        antOnCooldown = true;
        GameObject friendlyAnt = Instantiate(friendlyAntPrefab, new Vector3(friendlyAntSpawnPoint.x, friendlyAntSpawnPoint.y, 0f), Quaternion.identity);
        friendlyAnts.Add(friendlyAnt);

        antBar.rectTransform.sizeDelta = new Vector2(0, antBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < antCooldown)
        {
            antBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / antCooldown, antBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(antCooldown / 100);
            timeProgress += antCooldown / 100;
        }
        antOnCooldown = false;
    }

    private GameObject getClosestSand(GameObject ant)
    {
        GameObject closestSand = null;
        float shortestDistance = 100f;

        foreach (GameObject sand in sands)
        {
            if (sand != null)
            {
                float distance = (ant.transform.position - sand.transform.position).magnitude;
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestSand = sand;
                }
            }
        }

        return closestSand;
    }

    public static List<GameObject> getFriendlyAnts()
    {
        return friendlyAnts;
    }
}
