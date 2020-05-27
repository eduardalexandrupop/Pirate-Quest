using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public string[] spriteNames;
    public Sprite[] sprites;
    private Dictionary<string, Sprite> stringToSprite;

    // Start is called before the first frame update
    void Start()
    {
        stringToSprite = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            stringToSprite.Add(spriteNames[i], sprites[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getSprite(string spriteName)
    {
        return stringToSprite[spriteName];
    }
}
