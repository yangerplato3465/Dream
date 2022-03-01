using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    public RectTransform fade;
    public RectTransform redFade;
    public float fadeTime = 1f;
    public LeanTweenType fadeEaseType = LeanTweenType.easeInOutQuart;
    public GameObject restartButton;
    private int gemCount = 0;

    //Admobstuff
    private InterstitialAd interstitial;

    [Header("Level end panel")]
    public RectTransform levelEndPanel;
    public RectTransform levelText;
    public RectTransform completeText;
    public GameObject gem1;
    public GameObject gem2;
    public GameObject gem3;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;


    void Start() {
        AddListener();
        MobileAds.Initialize(initStatus => { 
            Debug.Log("admob initialized: ");
            RequestInterstitial();
        });
    }

    public void onRestartButtonClick() {
        fadeinRed();
        LeanTween.scale(restartButton, Vector3.one * 1.2f, 1).setEasePunch();
    }

    private void setCurrentLevel(object level) {
        currentLevel = (int)level;
    }

    private void onGameWin(object sender) {
        showLevelEndPanel();
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        fadeinRed();
        Debug.Log("Game lose");
    }

    private void onCollectGem(object sender) {
        gemCount += 1;
    }

    public void showLevelEndPanel() {
        levelEndPanel.gameObject.SetActive(true);
        LeanTween.scale(levelEndPanel.gameObject, Vector3.one * 1.1f, .6f).setEaseShake().setOnComplete(() => {
            particle1.Play();
            LeanTween.scale(levelText.gameObject, Vector3.one * 1.5f, .6f).setEasePunch().setOnComplete(() => {
                particle2.Play();
                particle3.Play();
                LeanTween.scale(completeText.gameObject, Vector3.one * 1.2f, .6f).setEasePunch().setOnComplete(showGemAnimation);
            });
        });
    }

    private void showGemAnimation() {
        if(gemCount == 0) return;
        gem1.SetActive(true);
        LeanTween.scale(gem1, Vector3.one * 1.5f, .6f).setEasePunch().setOnComplete(() => {
            if(gemCount > 1) {
                gem2.SetActive(true);
                LeanTween.scale(gem2, Vector3.one * 1.5f, .6f).setEasePunch().setOnComplete(() => {
                    if(gemCount > 2) {
                        gem3.SetActive(true);
                        LeanTween.scale(gem3, Vector3.one * 1.5f, .6f).setEasePunch();
                    }
                });
            }
        });
    }

    private void fadeinBlack() {
        LeanTween.alpha(fade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeoutBlack);
    }

    private void fadeoutBlack() {
        loadLevel();
        LeanTween.alpha(fade, 0f, fadeTime).setEase(fadeEaseType);
    }

    private void fadeinRed() {
        if(currentLevel == 0) {
            Debug.Log("Not in a level");
            return;
        }
        LeanTween.alpha(redFade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeoutRed);
    }

    private void fadeoutRed() {
        restartLevel();
        LeanTween.alpha(redFade, 0f, fadeTime).setEase(fadeEaseType);
    }

    private void restartLevel() {
        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel);
    }

    private void loadLevel() {
        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel + 1);
    }

    private void RequestInterstitial() {
        #if UNITY_ANDROID
            string adUnitId = Config.ANDROID_INTERSTITIAL;
        #elif UNITY_IPHONE
            string adUnitId = Config.IOS_INTERSTITIAL;
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    //Admob callbacks
    public void HandleOnAdLoaded(object sender, EventArgs args) {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        Debug.LogError("Ad failed to load: " + args.LoadAdError.GetCode());
        Debug.LogError("Ad failed to load message: " + args.LoadAdError.GetMessage());
    }

    public void HandleOnAdOpened(object sender, EventArgs args) {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args) {
        Debug.Log("HandleAdLeavingApplication event received");
    }

    public void ShowAd() {
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        } else {
            Debug.LogError("Ad failed to load");
        }
    }

    private void AddListener() {
        EventManager.AddListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.AddListener(SystemEvents.GAME_LOSE, onGameLose);
        EventManager.AddListener(SystemEvents.SET_LEVEL, setCurrentLevel);
        EventManager.AddListener(GamesEvents.COLLECT_GEM, onCollectGem);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.RemoveListener(SystemEvents.GAME_LOSE, onGameLose);
        EventManager.RemoveListener(SystemEvents.SET_LEVEL, setCurrentLevel);
        EventManager.RemoveListener(GamesEvents.COLLECT_GEM, onCollectGem);
    }
}
