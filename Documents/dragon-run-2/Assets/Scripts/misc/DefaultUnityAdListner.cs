using UnityEngine.Advertisements;
using UnityEngine;

public class DefaultUnityAdListener : MonoBehaviour
{
    public virtual void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public virtual void OnUnityAdsDidFinish()
    {
        Debug.Log("Ad finished ");
    }

    public virtual void OnUnityAdsDidStart(string placementId)
    {
    }

    public virtual void OnUnityAdsReady(string placementId)
    {
    }
}
