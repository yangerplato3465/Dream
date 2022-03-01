using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class MenuView : MonoBehaviour {

    public RectTransform shopPanel;
    public GameObject circleSwipe;
    private RewardedAd rewardedAd;

    private void Awake() {
        LeanTween.moveX(circleSwipe, 0f, 0f);
    }

    void Start() {
        Debug.Log("start functin");
        LeanTween.moveLocalX(circleSwipe, -2400f, 1f).setEaseOutQuad();
        CreateAndLoadAd();
    }

    private void CreateAndLoadAd() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
            string adUnitId = "unexpected_platform";
        #endif
        rewardedAd = new RewardedAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        Debug.Log("HandleRewardedAdFailedToLoad" + args.GetHashCode());
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.GetHashCode());
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        Debug.Log("HandleRewardedAdClosed event received");
        CreateAndLoadAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args){
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
    }

    public void ShowAd() {
        if (this.rewardedAd.IsLoaded()) {
            this.rewardedAd.Show();
        }
    }
    
    public void OnButtonClicked(string name) {
        Debug.Log(name);
        switch(name) {
            case "start":
                GoToLevelSelectScene();
                break;
            case "shop":
                OpenShopPanel();
                FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
                break;
            case "twitter":
                Application.OpenURL(Config.TWITTER_URL);
                break;
            case "itch":
                Application.OpenURL(Config.ITCH_URL);
                break;
            case "close":
                CloseShopPanel();
                FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
                break;
        }
    }

    private void OpenShopPanel() {
        shopPanel.gameObject.SetActive(true);
        LeanTween.scale(shopPanel.gameObject, Vector3.one * 1.1f, .8f).setEasePunch();
    }

    private void CloseShopPanel() {
        LeanTween.scale(shopPanel.gameObject, Vector3.one * .6f, .3f).setEaseInBack().setOnComplete(() => {
            shopPanel.gameObject.SetActive(false);
            LeanTween.scale(shopPanel.gameObject, Vector3.one * 1f, 0);
        });
    }

    public void OnToggleSound(Toggle disable) {
        AudioManager.ToggleAllSFX(disable.isOn);
        Debug.Log("Sound disable is " + disable.isOn);
    }

    private void GoToLevelSelectScene() {
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.LEVELSELECT_SCENE);
        });
    }
}
