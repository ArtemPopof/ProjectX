using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsShop : MonoBehaviour
{
    public List<Level> levels;
    private Level currentLevel;
    public Image cover;
    public ShopItemUi shopItem;

    // Start is called before the first frame update
    void Start()
    {
        var currentLevelIndex = PlayerPrefs.GetInt("currentLevel");
        currentLevel = levels[currentLevelIndex];

        //carousel.AddOnSwipeListener((currentIndex) => { UpdateCurrentLevel(currentIndex); });
    }

    private void UpdateCurrentLevel(int currentIndex)
    {
        currentLevel = levels[currentIndex];
        cover.sprite = currentLevel.shopCover;

        shopItem.UpdateItemInfo(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
