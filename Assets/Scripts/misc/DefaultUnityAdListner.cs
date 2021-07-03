using UnityEngine.Advertisements;
using UnityEngine;

public class DefaultUnityAdListener : MonoBehaviour, IUnityAdsListener
{
    public virtual void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public virtual void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad finished " + showResult);
    }

    public virtual void OnUnityAdsDidStart(string placementId)
    {
    }

    public virtual void OnUnityAdsReady(string placementId)
    {
    }
}
