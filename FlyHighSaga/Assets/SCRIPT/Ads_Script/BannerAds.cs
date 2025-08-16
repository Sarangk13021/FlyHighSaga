using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
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
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions()
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };
        Advertisement.Banner.Load(AdUnitId, options);
    }

    private void BannerLoadedError(string message) { }
    private void BannerLoaded()
    {
        Debug.Log("Banner ad loaded");
    }
    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions()
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };
        Advertisement.Banner.Show(AdUnitId, options);
    }
    private void BannerHidden() { }
    private void BannerClicked() { }
    private void BannerShown() { }
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }
}
