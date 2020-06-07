using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsShop : MonoBehaviour
{
    public List<Sprite> levelCovers;
    private Sprite currentSprite;
    public Carousel carousel;
    public Image cover;

    // Start is called before the first frame update
    void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentSprite = levelCovers[currentLevel];

        carousel.AddOnSwipeListener((currentIndex) => { UpdateCurrentLevelCover(currentIndex); });
    }

    private void UpdateCurrentLevelCover(int currentIndex)
    {
        currentSprite = levelCovers[currentIndex];
        cover.sprite = currentSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
