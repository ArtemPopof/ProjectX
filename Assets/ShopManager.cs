using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Linq;
using System;

public class ShopManager : DefaultUnityAdListener
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
    public List<Text> currentMoneyLabels;
    private int currentIndex = 0;

    public GameObject dragonShopUI;
    public GameObject mainShopUI;
    public GameObject buyMoneyDialogMain;
    // Dialog, that opens from dragons shop ui
    public GameObject buyMoneyDialogDragons;

    private GameObject currentScreen;

    void Start()
    {
        currentScreen = mainShopUI;
        UpdateCurrentBalance();
    }

    private void OnDragonShopUIStart()
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
        UpdateCurrentBalance();

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

    private void UpdateCurrentBalance()
    {
        var balance = PlayerPrefs.GetInt("coins").ToString();

        foreach (Text text in currentMoneyLabels)
        {
            text.text = balance;
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

    public void OpenDragonsShop()
    {
        dragonShopUI.SetActive(true);
        mainShopUI.SetActive(false);
        OnDragonShopUIStart();
    }

    public void ReturnToMainShopUI()
    {
        dragonShopUI.SetActive(false);
        mainShopUI.SetActive(true);
    }

    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished, adding coins " + showResult);

        if (!AdComponent.UserWatchedAd(showResult)) return;

        var currentCoins = PlayerPrefs.GetInt("coins");
        currentCoins += 300;
        PlayerPrefs.SetInt("coins", currentCoins);

        UpdateCurrentBalance();

        // Maybe some dataloss here?    
        PlayerPrefs.SetInt("inshopAdLastShow", (int) (DateTime.Now.Ticks / TimeSpan.TicksPerHour));
    }

    public void OpenBuyMoneyDialogFromMainShop()
    {
        buyMoneyDialogMain.SetActive(true);
    }

    public void OpenBuyMoneyDialogFromDragonsShop()
    {
        buyMoneyDialogDragons.SetActive(true);
    }

}
