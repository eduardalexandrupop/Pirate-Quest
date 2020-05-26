using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyComb : MonoBehaviour
{
    public GameObject collectBarPrefab;

    private Rigidbody2D rb;
    private bool beingCollected;
    private float collectRange = 0.8f;

    private GameObject playerInstance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        beingCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if ((playerInstance.transform.position - transform.position).magnitude < collectRange)
        {
            if (beingCollected == false)
            {
                beingCollected = true;
                StartCoroutine(collectHoney());
            }
        }
        else
        {
            beingCollected = false;
        }
    }

    public IEnumerator collectHoney()
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
                    BeesArenaManager.gatherHoney();
                    Destroy(gameObject);
                    GameObject.Destroy(collectBarInstance);
                }
                yield return new WaitForSeconds(0.07f);
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
