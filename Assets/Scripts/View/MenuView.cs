using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class MenuView : MonoBehaviour {

    public RectTransform shopPanel;
    public GameObject circleSwipe;
    public RectTransform title;
    public RectTransform sparkle;
    public RectTransform yellowAlien;
    public RectTransform brownAlien;
    public RectTransform blueAlien;
    public RectTransform greenAlien;
    public RectTransform redAlien;
    public GameObject startButton;
    public GameObject sfxButton;
    public GameObject shopButton;
    private RewardedAd rewardedAd;
    private string rewardType;
    
    [Header("Shop Buttons")]
    public Button greenButton;
    public Button blueButton;
    public Button redButton;
    public Button orangeButton;

    private void Awake() {
        LeanTween.moveX(circleSwipe, 0f, 0f);
    }

    void Start() {
        LeanTween.moveLocalX(circleSwipe, -3000f, 1f).setEaseOutQuad();
        TitleAnimation();
        CreateAndLoadAd();
        UpdateShop();
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
        if(rewardType == null) {
            Debug.LogWarning("Something went wrong while giving reward");
            return;
        }
        UnlockAliens(rewardType);
    }

    private void UnlockAliens(string type){
        switch(type){
            case "blue":
                PlayerPrefs.SetInt(PlayerprefConst.BLUE_UNLOCKED, 1);
                break;

            case "red":
                PlayerPrefs.SetInt(PlayerprefConst.RED_UNLOCKED, 1);
                break;

            case "orange":
                PlayerPrefs.SetInt(PlayerprefConst.ORANGE_UNLOCKED, 1);
                break;
        }
        UpdateShop();
    }

    private void UpdateShop() {
        if(PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE) == "") {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.GREEN);
        }
        string alienInUse = PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE);
        Text greenBtnText = greenButton.transform.GetChild(0).GetComponent<Text>();
        Text blueBtnText = blueButton.transform.GetChild(0).GetComponent<Text>();
        Text redBtnText = redButton.transform.GetChild(0).GetComponent<Text>();
        Text orangeBtnText = orangeButton.transform.GetChild(0).GetComponent<Text>();
        Transform blueAd = blueButton.transform.GetChild(1);
        Transform redAd = redButton.transform.GetChild(1);
        Transform orangeAd = orangeButton.transform.GetChild(1);

        greenBtnText.text = alienInUse == PlayerprefConst.GREEN ? "in use" : "use";
        blueBtnText.text = alienInUse == PlayerprefConst.BLUE ? "in use" : "use";
        redBtnText.text = alienInUse == PlayerprefConst.RED ? "in use" : "use";
        orangeBtnText.text = alienInUse == PlayerprefConst.ORANGE ? "in use" : "use";
        
        blueAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) != 1);
        redAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) != 1);
        orangeAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) != 1);
        blueBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) == 1);
        redBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) == 1);
        orangeBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) == 1);
    }

    public void ShowAd(string type) {
        if(type == PlayerprefConst.GREEN) {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.GREEN);
            UpdateShop();
            return;
        }
        if(type == PlayerprefConst.BLUE && PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) == 1) {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.BLUE);
            UpdateShop();
            return;
        }
        if(type == PlayerprefConst.RED && PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) == 1) {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.RED);
            UpdateShop();
            return;
        }
        if(type == PlayerprefConst.ORANGE && PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) == 1) {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.ORANGE);
            UpdateShop();
            return;
        }
        if(this.rewardedAd.IsLoaded()) {
            rewardType = type;
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
                Debug.Log("Alien in use is " + PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE));
                FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
                break;
        }
    }

    private void TitleAnimation() {
        LeanTween.alpha(title, 1f, .8f);
        LeanTween.moveX(title, 0f, 1f).setEaseOutQuad();
        LeanTween.alpha(sparkle, 1f, .3f).setDelay(.3f);
        LeanTween.scale(sparkle.gameObject, Vector3.one * 1.3f, 2f).setEasePunch();
        LeanTween.alpha(yellowAlien, 1f, .3f).setDelay(.3f);
        LeanTween.alpha(brownAlien, 1f, .3f).setDelay(.5f);
        LeanTween.alpha(blueAlien, 1f, .3f).setDelay(.7f);
        LeanTween.scale(yellowAlien.gameObject, new Vector3(-1, 1, 1) * 1.3f, 2f).setEasePunch().setDelay(.3f);
        LeanTween.scale(brownAlien.gameObject, Vector3.one * 1.3f, 2f).setEasePunch().setDelay(.5f);
        LeanTween.scale(blueAlien.gameObject, Vector3.one * 1.3f, 2f).setEasePunch().setDelay(.7f);
        LeanTween.alpha(redAlien, 1f, .3f).setDelay(1f);
        LeanTween.alpha(greenAlien, 1f, .3f).setDelay(1f);
        LeanTween.scale(redAlien.gameObject, Vector3.one * 1.5f, 1.5f).setEasePunch().setDelay(1f);
        LeanTween.scale(greenAlien.gameObject, new Vector3(-1, 1, 1) * 1.5f, 1.5f).setEasePunch().setDelay(1f);
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
