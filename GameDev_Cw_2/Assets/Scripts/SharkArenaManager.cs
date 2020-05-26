using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkArenaManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject sharkPrefab;
    public GameObject barrelPrefab;
    public Image attackBar;
    public Image specialAttackBar;
    public Image specialAttackImage;
    public Image[] lives;
    public Text dead;
    public Text timerText;

    private List<Vector2> spawnPoints;
    private List<Vector2> barrelSpawnPoints;

    private List<GameObject> barrels;


    private static GameObject playerInstance;

    private float timeBetweenWaves = 5f;
    private float arenaDuration = 60f;
    private float timer;

    private float startTime;

    private static int barrelsChecked;

    private static int barrelsOnMap;

    private static bool foundKey;

    // crocodile power
    public bool crocodileUnlocked;
    public Image crocodileBar;
    public Image crocodileImage;
    public GameObject friendlyCrocodilePrefab;

    private Vector2 friendlyCrocodileSpawnPoint;
    private float crocodileCooldown;
    private bool crocodileOnCooldown;

    private static List<GameObject> friendlyCrocodiles;

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

        if (crocodileUnlocked == false)
        {
            crocodileBar.gameObject.SetActive(false);
            crocodileImage.gameObject.SetActive(false);
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

        barrelSpawnPoints = new List<Vector2>();
        barrelSpawnPoints.Add(new Vector2(-5, -2));
        barrelSpawnPoints.Add(new Vector2(-5, 2));
        barrelSpawnPoints.Add(new Vector2(5, 2));
        barrelSpawnPoints.Add(new Vector2(5, -2));
        barrelSpawnPoints.Add(new Vector2(0, -2));
        barrelSpawnPoints.Add(new Vector2(-5, 2));

        barrels = new List<GameObject>();

        startTime = Time.time;
        timer = arenaDuration;
        InvokeRepeating("spawnEnemies", 4f, timeBetweenWaves);
        InvokeRepeating("spawnBarrels", 2f, 5f);

        barrelsChecked = 0;
        barrelsOnMap = 0;

        foundKey = false;

        // crocodile power
        friendlyCrocodileSpawnPoint = new Vector2(0, 5);
        crocodileCooldown = 15f;
        crocodileOnCooldown = false;

        friendlyCrocodiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        timer = arenaDuration - timePassed;
        if (timer < 0)
            timer = 0;

        displayTimer();


        // crocodile power
        if (crocodileOnCooldown == false && crocodileUnlocked)
        {
            if (Input.GetKey(KeyCode.R))
            {
                StartCoroutine(crocodilePower());
            }
        }
    }

    void FixedUpdate()
    {
        foreach (GameObject crocodile in friendlyCrocodiles)
        {
            if (crocodile != null)
                crocodile.GetComponent<EnemyAI>().target = getClosestBarrel(crocodile);
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
            enemy = Instantiate(sharkPrefab, new Vector3(spawnPoint1.x, spawnPoint1.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;

            enemy = Instantiate(sharkPrefab, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().target = playerInstance;
            enemy.GetComponent<EnemyAttack>().player = playerInstance;
        }
    }

    void spawnBarrels()
    {
        if (barrelsOnMap < 3 && foundKey == false)
        {
            Vector2 barrelSpawnPoint = barrelSpawnPoints[Random.Range(0, barrelSpawnPoints.Count)];

            GameObject barrel = Instantiate(barrelPrefab, new Vector3(barrelSpawnPoint.x, barrelSpawnPoint.y, 0), Quaternion.identity);
            barrel.GetComponent<Barrel>().setPlayerInstance(playerInstance);
            barrelsOnMap++;

            barrels.Add(barrel);
        }
    }

    void displayTimer()
    {
        if (timerText != null)
            timerText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - (int)timer / 60).ToString("D2");
    }

    public static void checkBarrel()
    {
        barrelsChecked++;
        barrelsOnMap--;

        if (barrelsChecked == 7)
            foundKey = true;
    }

    public static GameObject getPlayerInstance()
    {
        return playerInstance;
    }

    public static int getBarrelsChecked()
    {
        return barrelsChecked;
    }

    private IEnumerator crocodilePower()
    {
        crocodileOnCooldown = true;
        GameObject friendlyCrocodile = Instantiate(friendlyCrocodilePrefab, new Vector3(friendlyCrocodileSpawnPoint.x, friendlyCrocodileSpawnPoint.y, 0f), Quaternion.identity);
        friendlyCrocodiles.Add(friendlyCrocodile);

        crocodileBar.rectTransform.sizeDelta = new Vector2(0, crocodileBar.rectTransform.sizeDelta.y);
        float timeProgress = 0f;
        while (timeProgress < crocodileCooldown)
        {
            crocodileBar.rectTransform.sizeDelta = new Vector2(60 * timeProgress / crocodileCooldown, crocodileBar.rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(crocodileCooldown / 100);
            timeProgress += crocodileCooldown / 100;
        }
        crocodileOnCooldown = false;
    }

    private GameObject getClosestBarrel(GameObject crocodile)
    {
        GameObject closestBarrel = null;
        float shortestDistance = 100f;

        foreach (GameObject barrel in barrels)
        {
            if (barrel != null)
            {
                float distance = (crocodile.transform.position - barrel.transform.position).magnitude;
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestBarrel = barrel;
                }
            }
        }

        return closestBarrel;
    }

    public static List<GameObject> getFriendlyCrocodiles()
    {
        return friendlyCrocodiles;
    }
}
