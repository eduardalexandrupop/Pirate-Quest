using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    public GameObject collectBarPrefab;

    public Sprite chestSprite;

    private Rigidbody2D rb;
    private bool beingCollected;
    private GameObject collector;
    private float collectRange = 0.8f;

    private GameObject playerInstance;

    private bool isChest;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        beingCollected = false;

        isChest = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        bool visited = false;

        foreach (GameObject ant in SkeletonArenaManager.getFriendlyAnts())
        {
            if (ant != null && (ant.transform.position - transform.position).magnitude < collectRange && isChest == false)
            {
                visited = true;
                if (beingCollected == false)
                {
                    beingCollected = true;
                    collector = ant;
                    StartCoroutine(checkSand());
                }
            }
        }

        if ((playerInstance.transform.position - transform.position).magnitude < collectRange && isChest == false)
        {
            visited = true;
            if (beingCollected == false)
            {
                beingCollected = true;
                collector = playerInstance;
                StartCoroutine(checkSand());
            }
        }

        if (visited == false)
        {
            beingCollected = false;
            collector = null;
        }
    }

    public IEnumerator checkSand()
    {
        GameObject collectBarInstance = Instantiate(collectBarPrefab, transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
        collectBarInstance.transform.localScale = new Vector3(2f, 0.2f, 1f);
        float i;
        for (i = 0f; i <= 2.1f; i += 0.2f)
        {
            if (beingCollected == true)
            {
                collectBarInstance.transform.localScale = new Vector3(2 - i, 0.2f, 1f);
                if (collectBarInstance.transform.localScale.x <= 0f)
                {
                    SkeletonArenaManager.checkSand();
                    GameObject.Destroy(collectBarInstance);
                    if (SkeletonArenaManager.getSandChecked() == 7)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sprite = chestSprite;
                        isChest = true;

                        if (collector.name.Contains("Ant"))
                            Destroy(collector);
                    }
                    else
                    {
                        Destroy(gameObject);
                        if (collector.name.Contains("Ant"))
                            Destroy(collector);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                GameObject.Destroy(collectBarInstance);
                break;
            }
        }
    }

    public void setPlayerInstance(GameObject player)
    {
        playerInstance = player;
    }
}
