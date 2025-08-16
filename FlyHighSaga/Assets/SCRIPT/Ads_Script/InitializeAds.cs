using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class InitializeAds : MonoBehaviour ,IUnityAdsInitializationListener
{
    [SerializeField] private string AndroidGameId;
    [SerializeField] private string IosGameId;
    [SerializeField] private bool isTesting;

    private string Gameid;

    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }

    private void Awake()
    {
#if UNITY_IOS
Gameid = IosGameId;
#elif UNITY_ANDROID
Gameid = AndroidGameId;
#elif UNITY_EDITOR
Gameid = AndroidGameId; //if you havent switched platform;
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(Gameid, isTesting, this);
        }
    }
}
