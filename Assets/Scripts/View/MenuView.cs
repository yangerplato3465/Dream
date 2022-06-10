using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using Lean.Localization;

public class MenuView : MonoBehaviour {

    public RectTransform shopPanel;
    public RectTransform alertPanel;
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
    public GameObject musicButton;
    public GameObject shopButton;
    public Image soundToggle;
    public Image musicToggle;
    public Text version;
    public GameObject loading;
    private RewardedAd rewardedAd;
    private string rewardType;
    private Boolean giveReward = false;
    
    [Header("Shop Buttons")]
    public Button greenButton;
    public Button blueButton;
    public Button redButton;
    public Button orangeButton;
    public GameObject iapButton1;
    public GameObject iapButton2;

    private void Awake() {
        loadingSpin();
        LeanTween.moveX(circleSwipe, 0f, 0f);
    }

    void Start() {
        LeanTween.moveLocalX(circleSwipe, -3000f, 1f).setEaseOutQuad();
        version.text = "Ver - " + Application.version;
        TitleAnimation();
        CreateAndLoadAd();
        UpdateShop();
        if(PlayerPrefs.GetInt(PlayerprefConst.MUSIC) == 1) toggleMusic(true, true);
        if(PlayerPrefs.GetInt(PlayerprefConst.SFX) == 1) toggleSound(true, true);
    }

    private void Update() {
        if(giveReward && rewardType != null) {
            UnlockAliens(rewardType);
            giveReward = false;
        }
    }

    private void CreateAndLoadAd() {
        // #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        // #elif UNITY_IPHONE
        //     string adUnitId = "ca-app-pub-3940256099942544/1712485313";
        // #else
        //     string adUnitId = "unexpected_platform";
        // #endif
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
        Debug.LogWarning("HandleRewardedAdFailedToLoad" + args.GetHashCode());
        UpdateShop(true);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        Debug.Log("HandleRewardedAdOpening event received");
        AudioManager.ToggleMusic(true);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {
        Debug.LogWarning("HandleRewardedAdFailedToShow event received with message: " + args.GetHashCode());
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        Debug.Log("HandleRewardedAdClosed event received");
        AudioManager.ToggleMusic(PlayerPrefs.GetInt(PlayerprefConst.MUSIC) == 1);
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
        giveReward = true;
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

    private void UpdateShop(bool loadAdFailed = false) {
        if(PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE) == "") {
            PlayerPrefs.SetString(PlayerprefConst.ALIEN_IN_USE, PlayerprefConst.GREEN);
        }
        iapButton1.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ADFREE_UNLOCKED) != 1);
        iapButton2.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ADFREE_UNLOCKED) != 1);

        string alienInUse = PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE);
        Text greenBtnText = greenButton.transform.GetChild(0).GetComponent<Text>();
        Text blueBtnText = blueButton.transform.GetChild(0).GetComponent<Text>();
        Text redBtnText = redButton.transform.GetChild(0).GetComponent<Text>();
        Text orangeBtnText = orangeButton.transform.GetChild(0).GetComponent<Text>();
        Transform blueAd = blueButton.transform.GetChild(1);
        Transform redAd = redButton.transform.GetChild(1);
        Transform orangeAd = orangeButton.transform.GetChild(1);

        greenBtnText.text = alienInUse == PlayerprefConst.GREEN ? LeanLocalization.GetTranslationText("inuse") : LeanLocalization.GetTranslationText("use");
        blueBtnText.text = alienInUse == PlayerprefConst.BLUE ? LeanLocalization.GetTranslationText("inuse") : LeanLocalization.GetTranslationText("use");
        redBtnText.text = alienInUse == PlayerprefConst.RED ? LeanLocalization.GetTranslationText("inuse") : LeanLocalization.GetTranslationText("use");
        orangeBtnText.text = alienInUse == PlayerprefConst.ORANGE ? LeanLocalization.GetTranslationText("inuse") : LeanLocalization.GetTranslationText("use");
        
        blueAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) != 1);
        redAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) != 1);
        orangeAd.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) != 1);
        blueBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) == 1);
        redBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) == 1);
        orangeBtnText.gameObject.SetActive(PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) == 1);

        if(loadAdFailed) {
            if(PlayerPrefs.GetInt(PlayerprefConst.BLUE_UNLOCKED) == 0) {setAdButtonUnavailable(blueButton);}
            if(PlayerPrefs.GetInt(PlayerprefConst.RED_UNLOCKED) == 0) {setAdButtonUnavailable(redButton);}
            if(PlayerPrefs.GetInt(PlayerprefConst.ORANGE_UNLOCKED) == 0) {setAdButtonUnavailable(orangeButton);}
        }
    }

    private void setAdButtonUnavailable(Button button) {
        // Text text = button.transform.GetChild(1).GetComponentInChildren<Text>();
        // text.text = LeanLocalization.GetTranslationText("notavail");
        button.interactable = false;
    }

    public void ShowAd(string type) {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
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
        switch(name) {
            case "start":
                GoToLevelSelectScene();
                break;
            case "shop":
                OpenShopPanel();
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
                break;
            case "closealert":
                CloseAlertPanel();
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
        LeanTween.scale(redAlien.gameObject, Vector3.one * 1.2f, 1.5f).setEasePunch().setDelay(1f);
        LeanTween.scale(greenAlien.gameObject, new Vector3(-1, 1, 1) * 1.2f, 1.5f).setEasePunch().setDelay(1f);
    }

    private void OpenShopPanel() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        shopPanel.gameObject.SetActive(true);
        LeanTween.scale(shopPanel.gameObject, Vector3.one * 1.1f, .8f).setEasePunch();
    }

    private void CloseShopPanel() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
        LeanTween.scale(shopPanel.gameObject, Vector3.one * .6f, .3f).setEaseInBack().setOnComplete(() => {
            shopPanel.gameObject.SetActive(false);
            LeanTween.scale(shopPanel.gameObject, Vector3.one * 1f, 0);
        });
    }

    private void OpenAlertPanel() {
        alertPanel.gameObject.SetActive(true);
        LeanTween.scale(alertPanel.gameObject, Vector3.one * 1.1f, .8f).setEasePunch();
    }

    private void CloseAlertPanel() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
        LeanTween.scale(alertPanel.gameObject, Vector3.one * .6f, .3f).setEaseInBack().setOnComplete(() => {
            alertPanel.gameObject.SetActive(false);
            LeanTween.scale(alertPanel.gameObject, Vector3.one * 1f, 0);
        });
        UpdateShop();
    }

    public void OnToggleSound(Toggle toggle) {
        toggleSound(toggle.isOn);
    }

    private void toggleSound(bool disable, bool isManual = false) {
        if(isManual)  sfxButton.GetComponent<Toggle>().isOn = disable;
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        AudioManager.ToggleAllSFX(disable);
        var tempColor = soundToggle.color;
        tempColor.a = disable ? 0f : 1f;
        soundToggle.color = tempColor;
        PlayerPrefs.SetInt(PlayerprefConst.SFX, disable ? 1 : 0); //0 = unmute, 1 = mute
    }

    public void OnToggleMusic(Toggle toggle) {
        toggleMusic(toggle.isOn);
    }

    private void toggleMusic(bool disable, bool isManual = false) {
        if(isManual)  musicButton.GetComponent<Toggle>().isOn = disable;
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        AudioManager.ToggleMusic(disable);
        var tempColor = musicToggle.color;
        tempColor.a = disable ? 0f : 1f;
        musicToggle.color = tempColor;
        PlayerPrefs.SetInt(PlayerprefConst.MUSIC, disable ? 1 : 0);
    }

    public void OnPurchaseSuccess() {
        PlayerPrefs.SetInt(PlayerprefConst.BLUE_UNLOCKED, 1);
        PlayerPrefs.SetInt(PlayerprefConst.RED_UNLOCKED, 1);
        PlayerPrefs.SetInt(PlayerprefConst.ORANGE_UNLOCKED, 1);
        PlayerPrefs.SetInt(PlayerprefConst.ADFREE_UNLOCKED, 1);
        OpenAlertPanel();
    }

    public void OnPurchaseFailed() {
        Debug.Log("Something went wrong");
    }

    public void loadingSpin() {
        LeanTween.rotateZ(loading, 180f, 1f).setEaseOutElastic().setDelay(.5f).setOnComplete(() => {
            LeanTween.rotateZ(loading, 360f, 1f).setEaseOutElastic().setDelay(.5f).setOnComplete(loadingSpin);
        });
    }

    public void OnAlienClick(GameObject alien) {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        alien.GetComponent<Button>().interactable = false;
        Vector3 scale = alien.transform.localScale.x == 1 ? Vector3.one * 1.2f : new Vector3(-1, 1, 1) * 1.2f;
        LeanTween.scale(alien, scale, .8f).setEasePunch().setOnComplete(() => {
            alien.GetComponent<Button>().interactable = true;
        });
    }

    private void GoToLevelSelectScene() {
        FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.LEVELSELECT_SCENE);
        });
    }
}
