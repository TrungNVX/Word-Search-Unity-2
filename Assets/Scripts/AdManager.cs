using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public string appID;
    public string adBannerID;
    public string adIntersitialID;
    public AdPosition bannerPos;
    //public bool isTestDevice = false;
    //public List<string> testDevices = new List<string>();

    private BannerView bannerView;
    private InterstitialAd interstitial;
    public static AdManager Instance;
    

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }
    private void RequestInterstitial()
    {
        // Clean up interstitial before using it
        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        AdRequest request = new AdRequest.Builder().Build();
        InterstitialAd.Load(appID, request, (InterstitialAd ad, LoadAdError loadAdError) =>
        {
            if (loadAdError != null)
            {
                return;
            }
            else
            if (ad == null)
            {
                return;
            }
            Debug.Log("Interstitial ad loaded");
            interstitial = ad;

        });
    }
    public void ShowInterstitialAd()  // Show
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
        }
    }
    //public void CreateBanner(AdRequest request)
    //{
    //    this.bannerView = new BannerView(adBannerID, AdSize.SmartBanner,this.bannerPos);
    //}

}
