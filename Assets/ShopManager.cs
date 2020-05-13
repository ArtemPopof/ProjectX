using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ShopManager : MonoBehaviour
{
    public Carousel carousel;
    public Transform dragons;
    private Dragon currentDragon;
    public Text characterName;
    public Text characterPrice;
    public Button buyButton;
    public Transform beforeBuy;
    public Transform afterBuy;
    public Text buyText;
    public Text moneyOnAccount;
    private int currentIndex = 0;

    void Start()
    {
        currentIndex = PlayerPrefs.GetInt("characterLook");
        carousel.AddOnSwipeListener((currentIndex) => { DragonChooseChanged(currentIndex); });
        carousel.SetIndex(currentIndex);
        DragonChooseChanged(currentIndex);
    }

    private void DragonChooseChanged(int newIndex)
    {
        currentIndex = newIndex;

        var childrenDragons = dragons.Cast<Transform>().ToArray();
        currentDragon = childrenDragons[currentIndex].GetComponent<Dragon>();

        UpdateDragonInfo(currentDragon);
    }

    private void UpdateDragonInfo(Dragon dragon)
    {
        characterPrice.text = dragon.price.ToString();
        characterName.text = dragon.name;
        moneyOnAccount.text = PlayerPrefs.GetInt("coins").ToString();

        if (AlreadyBought(currentIndex))
        {
            UpdateAfterBuyInfo();
        } else
        {
            afterBuy.gameObject.SetActive(false);
            beforeBuy.gameObject.SetActive(true);

            buyButton.interactable = IsAbleToBuy(dragon.price);
            if (IsAbleToBuy(dragon.price))
            {
                UpdateButtonTextForAvailabilityToBuy(true);
            }
            else
            {
                UpdateButtonTextForAvailabilityToBuy(false);
            }
        }
    }

    private bool AlreadyBought(int index)
    {
        var availableLooks = PlayerPrefs.GetString("availableLooks").Split(';');
        foreach (var look in availableLooks)
        {
            if (look == index.ToString())
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateAfterBuyInfo()
    {
        afterBuy.gameObject.SetActive(true);
        beforeBuy.gameObject.SetActive(false);

        var text = afterBuy.GetComponentInChildren<Text>();
        Debug.Log("CurrentIndex: " + currentIndex);
        if (currentIndex == PlayerPrefs.GetInt("characterLook"))
        {
            text.text = "Current";
            buyButton.interactable = false;
        } else
        {
            text.text = "Choose";
            buyButton.interactable = true;
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

    public void ReturnButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sprint3");
    }

    public void BuyButtonClicked()
    {
        if (AlreadyBought(currentIndex))
        {
            PlayerPrefs.SetInt("characterLook", currentIndex);
            DragonChooseChanged(currentIndex);
            return;
        }

        AdjustCoinCount();
        AdjustCurrentCharacterModel();

        DragonChooseChanged(currentIndex);
    }

    private void AdjustCurrentCharacterModel()
    {
        PlayerPrefs.SetInt("characterLook", currentIndex);
        var availableLooks = PlayerPrefs.GetString("availableLooks");
        availableLooks += ";" + currentIndex.ToString();
        PlayerPrefs.SetString("availableLooks", availableLooks);
    }

    private void AdjustCoinCount()
    {
        var playerCoins = PlayerPrefs.GetInt("coins");
        playerCoins -= currentDragon.price;
        PlayerPrefs.SetInt("coins", playerCoins);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
