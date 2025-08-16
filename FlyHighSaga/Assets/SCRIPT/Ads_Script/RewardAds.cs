using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardAds : MonoBehaviour , IUnityAdsLoadListener , IUnityAdsShowListener
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
    public void LoadRewardAds()
    {
        Advertisement.Load(AdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstatial Ad loaded");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    public void ShowRewardAd()
    {
        Advertisement.Show(AdUnitId, this);
        LoadRewardAds();
    }
    #region ShowCallBacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == AdUnitId && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            Debug.Log("Ad fully watched");
        }
    }
    #endregion
}
