using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdComponent : MonoBehaviour
{
    public static string BLOCK_ID = "ca-app-pub-4835916624633322/6488470890";
    public static string TEST_ID = "ca-app-pub-3940256099942544/5224354917";
    public AdType type;
    public int maxShowCount;
    public RewardedAd rewardedAd;
    public DefaultUnityAdListener listener;
    public GameObject button;

    public static AdComponent Instance { private set; get; }

    private void Awake()
    {
        Debug.Log("AWAKE");
    }

    void Start()
    {
        ConfigureNextAd();
        UpdateAdAvailability();

        Debug.Log("START");
    }

    private void ConfigureNextAd()
    {
        rewardedAd = new RewardedAd(BLOCK_ID);
        SetCallbacks();
        LoadNextAd();
    }

    private void SetCallbacks()
    {
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad closed");
        ConfigureNextAd();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        Debug.Log("Ad finished with reward");

        if (type == AdType.RESUME_AD)
        {
            GameManager.Instance.SetUIPanelActive("DialogsUI", false);
            GameManager.Instance.Resurrect();
        }
        if (listener != null)
        {
            listener.OnUnityAdsDidFinish();
        }

        // exeeded max resurection ad count
        if (type == AdType.RESUME_AD && AdManager.CantShowAnotherResumeAd())
        {
            var buttonComponent = button.GetComponent<Button>();
            buttonComponent.interactable = false;
        }

        UpdateAdAvailability();
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Debug.LogError(e.Message);
        ConfigureNextAd();
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Debug.LogError(e.Message);
    }

    private void LoadNextAd()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }

    private void UpdateAdAvailability()
    {
        if (type == AdType.INSHOP_AD)
        {
            // timestamp in hours (to fit int size)
            var lastViewDate = PlayerPrefs.GetInt("inshopAdLastShow");
            var nowHoursTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerHour;
            var diff = nowHoursTimestamp - lastViewDate;

            if (diff < 1)
            {
                button.SetActive(false);
                return;
            }
        }
    }

    public void Show()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public enum AdType
    {
        RESUME_AD,
        INSHOP_AD
    }
}
