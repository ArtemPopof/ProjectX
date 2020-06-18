﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Linq;
using System;

public class ShopManager : DefaultUnityAdListener
{
    public Transform dragons;
    private int currentIndex = 0;

    public GameObject dragonShopUI;
    public GameObject levelShopUI;
    public GameObject mainShopUI;
    public GameObject buyMoneyDialog;
    public GameObject purchaseStatusDialog;
    public List<GameObject> shopModels;
    public List<Text> currentMoneyLabels;

    private GameObject currentScreen;

    void Start()
    {
        currentScreen = mainShopUI;
        UpdateCurrentBalance();

        GameManager.InitAdsEngine();
    }

    
    private void DragonChooseChanged(int newIndex)
    {
        currentIndex = newIndex;

        var childrenDragons = dragons.Cast<Transform>().ToArray();
        var currentDragon = childrenDragons[currentIndex].GetComponent<ShopItem>();

        //shopItemUi.UpdateItemInfo(currentDragon);
    }

    public void ReturnButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sprint3");
    }

    private void AdjustCurrentCharacterModel()
    {
        PlayerPrefs.SetInt("characterLook", currentIndex);
        var availableLooks = PlayerPrefs.GetString("availableLooks");
        availableLooks += ";" + currentIndex.ToString();
        PlayerPrefs.SetString("availableLooks", availableLooks);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDragonsShop()
    {
        dragonShopUI.SetActive(true);
        mainShopUI.SetActive(false);
        //OnDragonShopUIStart();
    }

    public void ReturnToMainShopUI()
    {
        dragonShopUI.SetActive(false);
        levelShopUI.SetActive(false);
        mainShopUI.SetActive(true);
    }

    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished, adding coins " + showResult);

        if (!AdComponent.UserWatchedAd(showResult)) return;

        SoundManager.PlaySound("Chest");

        var currentCoins = PlayerPrefs.GetInt("coins");
        currentCoins += 300;
        PlayerPrefs.SetInt("coins", currentCoins);

        UpdateCurrentBalance();

        // Maybe some dataloss here?    
        PlayerPrefs.SetInt("inshopAdLastShow", (int) (DateTime.Now.Ticks / TimeSpan.TicksPerHour));
    }

    public void OpenBuyMoneyDialog()
    {
        foreach (GameObject model in shopModels) {
            model.SetActive(false);
        }
        buyMoneyDialog.SetActive(true);
    }

    public void CloseBuyMoneyDialog()
    {
        buyMoneyDialog.SetActive(false);
        purchaseStatusDialog.SetActive(false);
        foreach (GameObject model in shopModels)
        {
            model.SetActive(true);
        }
        UpdateCurrentBalance();
    }

    public void OpenLevelShop()
    {
        levelShopUI.SetActive(true);
        mainShopUI.SetActive(false);
    }

    public void UpdateCurrentBalance()
    {
        var balance = PlayerPrefs.GetInt("coins").ToString();

        foreach (Text text in currentMoneyLabels)
        {
            text.text = balance;
        }
    }
}
