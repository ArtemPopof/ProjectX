using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdComponent : DefaultUnityAdListener
{
    public AdType type;
    public int maxShowCount;
    public DefaultUnityAdListener listener;

    public static AdComponent Instance { private set; get; }

    void Start()
    {
        Advertisement.AddListener(this);

        UpdateAdAvailability();
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
                transform.gameObject.SetActive(false);
                return;
            }
        }
    }

    public void Show()
    {
        if (!Advertisement.IsReady())
        {
            return;
        }

        Advertisement.Show();

        // exeeded max resurection ad count
        if (type == AdType.RESUME_AD && AdManager.CantShowAnotherResumeAd())
        {
            transform.gameObject.SetActive(false);
        }
    }

    public override void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);

        if (type == AdType.RESUME_AD)
        {

        }
    }

    public override void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished " + showResult);

        if (!UserWatchedAd(showResult)) return;

        if (listener != null) listener.OnUnityAdsDidFinish(placementId, showResult);

        if (type == AdType.RESUME_AD)
        {
            GameManager.Instance.Resurrect();
        }

        UpdateAdAvailability();
    }

    public static bool UserWatchedAd(ShowResult showResult)
    {
        return showResult == ShowResult.Finished || showResult == ShowResult.Failed;
    }

    public enum AdType
    {
        RESUME_AD,
        INSHOP_AD
    }
}
