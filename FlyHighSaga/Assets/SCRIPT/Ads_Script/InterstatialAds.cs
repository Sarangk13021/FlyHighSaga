using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstatialAds : MonoBehaviour , IUnityAdsLoadListener,IUnityAdsShowListener
{
    [SerializeField] private string AndroidAdUnitId;
    [SerializeField] private string IosAdUnitId;

    private string AdUnitId;

    private void Awake()
    {
#if UNITY_IOS
    AdUnitId = IosAdUnitId;
#elif UNITY_ANDROID
        AdUnitId = AndroidAdUnitId;
#endif
    }

    public void LoadInterstatialAds()
    {
        Advertisement.Load(AdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstatial Ad loaded");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    public void ShowInterstatialAd()
    {
        Advertisement.Show(AdUnitId, this);
        LoadInterstatialAds();
    }
    #region ShowCallBacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Interstatial ad completed");
    }
    #endregion
}
