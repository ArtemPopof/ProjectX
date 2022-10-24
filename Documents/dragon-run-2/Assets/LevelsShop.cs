using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsShop : MonoBehaviour
{
    public Image cover;
    public ShopItemUi shopItem;
    public Carousel carousel;

    // Start is called before the first frame update
    void Awake()
    {
        carousel.AddOnSwipeListener((newLevel) => { UpdateCurrentLevel(newLevel as Level); });
    }

    private void UpdateCurrentLevel(Level level)
    {
        cover.sprite = level.shopCover;

        shopItem.UpdateItemInfo(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
