using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public Carousel carousel;
    public Transform dragons;
    private Dragon currentDragon;
    public Text characterName;
    public Text characterPrice;
    public Button buyButton;
    public Text buyText;
    public Text moneyOnAccount;

    void Start()
    {
        carousel.AddOnSwipeListener((currentIndex) => { DragonChooseChanged(currentIndex); });
        DragonChooseChanged(0);
        moneyOnAccount.text = PlayerPrefs.GetInt("coins").ToString();
    }

    private void DragonChooseChanged(int currentIndex)
    {
        var childrenDragons = dragons.Cast<Transform>().ToArray();
        currentDragon = childrenDragons[currentIndex].GetComponent<Dragon>();

        UpdateDragonInfo(currentDragon);
    }

    private void UpdateDragonInfo(Dragon dragon)
    {
        characterPrice.text = dragon.price.ToString();
        characterName.text = dragon.name;
        buyButton.interactable = IsAbleToBuy(dragon.price);
        if (IsAbleToBuy(dragon.price))
        {
            UpdateButtonTextForAvailabilityToBuy(true);
        } else
        {
            UpdateButtonTextForAvailabilityToBuy(false);
        }
    }

    private bool IsAbleToBuy(int price)
    {
        var playerCoins = PlayerPrefs.GetInt("coins");
        return playerCoins >= price;
    }

    private void UpdateButtonTextForAvailabilityToBuy(bool canBuy)
    {
        if (canBuy)
        {
            buyText.text = "Buy for  ";
        } else
        {
            buyText.text = "You need  ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
