using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject collectBarPrefab;

    public Sprite keySprite;

    private Rigidbody2D rb;
    private bool beingCollected;
    private GameObject collector;
    private float collectRange = 0.8f;

    private GameObject playerInstance;

    private bool isKey;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        beingCollected = false;

        isKey = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        bool visited = false;

        foreach (GameObject crocodile in SharkArenaManager.getFriendlyCrocodiles())
        {
            if (crocodile != null && (crocodile.transform.position - transform.position).magnitude < collectRange && isKey == false)
            {
                visited = true;
                if (beingCollected == false)
                {
                    beingCollected = true;
                    collector = crocodile;
                    StartCoroutine(checkBarrel());
                }
            }
        }

        if ((playerInstance.transform.position - transform.position).magnitude < collectRange && isKey == false)
        {
            visited = true;
            if (beingCollected == false)
            {
                beingCollected = true;
                collector = playerInstance;
                StartCoroutine(checkBarrel());
            }
        }

        if (visited == false)
        {
            beingCollected = false;
            collector = null;
        }
    }

    public IEnumerator checkBarrel()
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
                    SharkArenaManager.checkBarrel();
                    GameObject.Destroy(collectBarInstance);
                    if (SharkArenaManager.getBarrelsChecked() == 7)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sprite = keySprite;
                        isKey = true;

                        if (collector.name.Contains("Crocodile"))
                            Destroy(collector);
                    }
                    else
                    {
                        Destroy(gameObject);
                        if (collector.name.Contains("Crocodile"))
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
