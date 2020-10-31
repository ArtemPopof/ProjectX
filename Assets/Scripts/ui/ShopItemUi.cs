using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * For use in shops, display info about item (price, name, etc)
 * manage buy button (disable, enable, change text)
 */
public class ShopItemUi : MonoBehaviour
{
    public Text balance;
    public Text characterName;
    public Text characterPrice;
    public Button buyButton;
    public Transform beforeBuy;
    public Transform afterBuy;
    public Text buyText;
    public Carousel carousel;
    public Type itemType;

    private ShopItem currentItem;

    void Start()
    {
        carousel.AddOnSwipeListener((item) => { UpdateItemInfo(item); });

        var currentItemIndex = 0;
        if (itemType == Type.LEVELS)
        {
            currentItemIndex = PlayerPrefs.GetInt("currentLevel") - 100;    // 100 looks like kostyl =(
        } else if (itemType == Type.DRAGONS)
        {
            currentItemIndex = PlayerPrefs.GetInt("characterLook");
        }

        carousel.SetIndex(currentItemIndex);
    }

    public void UpdateItemInfo(ShopItem item)
    {
        balance.text = PlayerPrefs.GetInt("coins").ToString();
        currentItem = item;
        if (characterPrice != null && item != null)
        {
            characterPrice.text = item.price.ToString();
        }
        if (characterName != null)
        {
            characterName.text = item.name;
        }

        if (AlreadyBought(item.index))
        {
            UpdateAfterBuyInfo(item.index);
        }
        else
        {
            afterBuy.gameObject.SetActive(false);
            beforeBuy.gameObject.SetActive(true);

            buyButton.interactable = IsAbleToBuy(item.price);
            if (IsAbleToBuy(item.price))
            {
                UpdateButtonTextForAvailabilityToBuy(true);
            }
            else
            {
                UpdateButtonTextForAvailabilityToBuy(false);
            }
        }
    }

    public bool AlreadyBought(int index)
    {
        var availableItems = PlayerPrefs.GetString("availableItems").Split(';');
        foreach (var item in availableItems)
        {
            if (item == index.ToString())
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateAfterBuyInfo(int itemIndex)
    {
        afterBuy.gameObject.SetActive(true);
        beforeBuy.gameObject.SetActive(false);

        var text = afterBuy.GetComponentInChildren<Text>();
        Debug.Log("CurrentIndex: " + itemIndex);
        if (itemIndex == PlayerPrefs.GetInt("characterLook") || itemIndex == PlayerPrefs.GetInt("currentLevel"))
        {
            text.text = "Current";
            buyButton.interactable = false;
        }
        else
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
        }
        else
        {
            buyText.text = "You need  ";
        }
    }

    public void BuyButtonClicked()
    {
        if (AlreadyBought(currentItem.index))
        {
            UpdateCurrentDragonOrLvl();
            UpdateItemInfo(currentItem);
            return;
        }

        AdjustCoinCount(currentItem.price);

        var items = PlayerPrefs.GetString("availableItems");
        items += ";" + currentItem.index.ToString();
        PlayerPrefs.SetString("availableItems", items);

        UpdateCurrentDragonOrLvl();
        UpdateItemInfo(currentItem);
    }

    public void RefreshUI()
    {
        UpdateItemInfo(currentItem);
    }

    private void AdjustCoinCount(int price)
    {
        var playerCoins = PlayerPrefs.GetInt("coins");
        playerCoins -= price;
        PlayerPrefs.SetInt("coins", playerCoins);
    }

    public void UpdateCurrentDragonOrLvl()
    {
        if (currentItem is Level)
        {
            PlayerPrefs.SetInt("currentLevel", currentItem.index);
        } else
        {
            PlayerPrefs.SetInt("characterLook", currentItem.index);
        }
    }

    public enum Type
    {
        DRAGONS,
        LEVELS
    }

}