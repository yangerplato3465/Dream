using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    public GameObject circleSwipe;
    public RectTransform fade;
    public RectTransform redFade;
    public float fadeTime = 1f;
    public LeanTweenType fadeEaseType = LeanTweenType.easeInOutQuart;
    public GameObject restartButton;
    private int gemCount = 0;
    private float timeElasped = 0;
    private bool timeToShowAd = false;

    //Admobstuff
    private InterstitialAd interstitial;
    private bool isAdFree = false;

    [Header("Level end panel")]
    public RectTransform levelEndPanel;
    public RectTransform gameEndPanel;
    public RectTransform levelText;
    public RectTransform completeText;
    public GameObject gem1;
    public GameObject gem2;
    public GameObject gem3;
    public GameObject homeButton;
    public GameObject nextLevelButton;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;

    private void Awake() {
        LeanTween.moveX(circleSwipe, 0f, 0f);
        AddListener();
    }
    void Start() {
        LeanTween.moveLocalX(circleSwipe, 3000f, 1f).setEaseOutQuad();
        isAdFree = PlayerPrefs.GetInt(PlayerprefConst.ADFREE_UNLOCKED) == 1;
        if(isAdFree) return;
        MobileAds.Initialize(initStatus => { 
            Debug.Log("admob initialized: ");
            RequestInterstitial();
        });
    }

    private void Update() {
        if(isAdFree) return;
        if(timeElasped < Config.TIME_TO_SHOW_AD) timeElasped += Time.deltaTime;
        else timeToShowAd = true;
    }

    public void onRestartButtonClick() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        fadeinRed();
        LeanTween.scale(restartButton, Vector3.one * 1.2f, 1).setEasePunch();
    }

    public void onMenuButtonClick() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.MENU_SCENE);
        });
    }

    private void setCurrentLevel(object level) {
        Debug.Log("setCurrentLevel" + (int)level);
        currentLevel = (int)level;
    }

    private void onGameWin(object sender) {
        showLevelEndPanel();
        if(gemCount > PlayerPrefs.GetInt("level" + currentLevel)) {
            PlayerPrefs.SetInt("level" + currentLevel, gemCount);
        }
        if(currentLevel >= PlayerPrefs.GetInt(PlayerprefConst.CURRENT_AVAIL_LEVEL)){
            PlayerPrefs.SetInt(PlayerprefConst.CURRENT_AVAIL_LEVEL, currentLevel + 1);
        }
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        fadeinRed();
        Debug.Log("Game lose");
    }

    private void onCollectGem(object sender) {
        gemCount += 1;
        if(gemCount == 1) FindObjectOfType<AudioManager>().Play(SoundConst.COLLECT_GEM1);
        if(gemCount == 2) FindObjectOfType<AudioManager>().Play(SoundConst.COLLECT_GEM2);
        if(gemCount == 3) FindObjectOfType<AudioManager>().Play(SoundConst.COLLECT_GEM3);

    }

    public void showLevelEndPanel() {
        levelEndPanel.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play(SoundConst.APPLAUSE);
        LeanTween.scale(levelEndPanel.gameObject, Vector3.one * 1.1f, .6f).setEaseShake().setOnComplete(() => {
            particle1.Play();
            LeanTween.scale(levelText.gameObject, Vector3.one * 1.5f, .3f).setEasePunch().setOnComplete(() => {
                particle2.Play();
                particle3.Play();
                LeanTween.scale(completeText.gameObject, Vector3.one * 1.2f, .3f).setEasePunch().setOnComplete(showGemAnimation);
            });
        });
    }

    private void closeLevelEndPanel() {
        LeanTween.scale(levelEndPanel.gameObject, Vector3.one * .6f, .3f).setEaseInBack().setOnComplete(() => {
            levelEndPanel.gameObject.SetActive(false);
            LeanTween.scale(levelEndPanel.gameObject, Vector3.one * 1f, 0);
            showGameEndPanel();
        });
    }

    public void showGameEndPanel() {
        gameEndPanel.gameObject.SetActive(true);
        LeanTween.scale(gameEndPanel.gameObject, Vector3.one * 1.1f, .6f).setEaseShake();
    }

    private void showGemAnimation() {
        if(gemCount == 0) {
            showLevelEndButtons();
            return;
        }
        gem1.SetActive(true);
        LeanTween.scale(gem1, Vector3.one * 1.5f, .3f).setEasePunch().setOnComplete(() => {
            if(gemCount > 1) {
                gem2.SetActive(true);
                LeanTween.scale(gem2, Vector3.one * 1.5f, .3f).setEasePunch().setOnComplete(() => {
                    if(gemCount > 2) {
                        gem3.SetActive(true);
                        LeanTween.scale(gem3, Vector3.one * 1.5f, .3f).setEasePunch().setOnComplete(showLevelEndButtons);
                    } else { showLevelEndButtons(); }
                });
            } else { showLevelEndButtons(); }
        });
    }

    private void showLevelEndButtons() {
        homeButton.SetActive(true);
        nextLevelButton.SetActive(true);
        LeanTween.scale(homeButton, Vector3.one * 1.2f, .6f).setEasePunch();
        LeanTween.scale(nextLevelButton, Vector3.one * 1.2f, .6f).setEasePunch();
    }

    public void fadeinBlack() { //click event for next level button
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        if(currentLevel >= Config.LEVEL_COUNT) {
            closeLevelEndPanel();
            return;
        }
        if(timeToShowAd) {
            ShowAd();
            return;
        }
        homeButton.SetActive(false);
        nextLevelButton.SetActive(false);
        LeanTween.alpha(fade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeoutBlack);
    }

    private void fadeoutBlack() {
        loadNextLevel();
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
        gemCount = 0;
        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel);
    }

    private void loadNextLevel() {
        Debug.Log("currentLevel" + currentLevel);

        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel + 1);
        Debug.Log("load level" + (currentLevel + 1));
        levelEndPanel.gameObject.SetActive(false);
        gemCount = 0;
        gem1.SetActive(false);
        gem2.SetActive(false);
        gem3.SetActive(false);
    }

    private void RequestInterstitial() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
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
        AudioManager.ToggleMusic(true);
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        Debug.Log("HandleAdClosed event received");
        LeanTween.alpha(fade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeoutBlack);
        AudioManager.ToggleMusic(PlayerPrefs.GetInt(PlayerprefConst.MUSIC) == 1);
        RequestInterstitial();
        timeElasped = 0f;
        timeToShowAd = false;
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args) {
        Debug.Log("HandleAdLeavingApplication event received");
    }

    public void ShowAd() {
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        } else {
            Debug.LogError("Ad failed to load");
            //continue gameplay if failed to load ad
            LeanTween.alpha(fade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeoutBlack);
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
