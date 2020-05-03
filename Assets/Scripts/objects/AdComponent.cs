using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdComponent : MonoBehaviour, IUnityAdsListener
{
    public AdType type;
    public int maxShowCount;

    void Start()
    {
        Advertisement.AddListener(this);
    }

    public void ShowAdToResume()
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
            return;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);

        if (type == AdType.RESUME_AD)
        {

        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished " + showResult);
        if (type == AdType.RESUME_AD && UserWatchedAd(showResult))
        {
            GameManager.Instance.Resurrect();
        }
    }

    private bool UserWatchedAd(ShowResult showResult)
    {
        return showResult == ShowResult.Finished || showResult == ShowResult.Failed;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public enum AdType
    {
        RESUME_AD,
        REWARD_AD
    }
}
