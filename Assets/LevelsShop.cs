using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsShop : MonoBehaviour
{
    public List<Sprite> levelCovers;
    private Sprite currentSprite;

    // Start is called before the first frame update
    void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentSprite = levelCovers[currentLevel];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
