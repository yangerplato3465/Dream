using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashView : MonoBehaviour {

    public RectTransform logo;
    public RectTransform text;
    public RectTransform earphone;
    public RectTransform earphoneText;
    public GameObject circleSwipe;
    public GameObject loading;

    void Start() {
        logoFadeIn();
        loadingSpin();
    }

    private void logoFadeIn() {
        LeanTween.alpha(logo, 1f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(.5f).setOnComplete(logoFadeOut);
        LeanTween.alpha(text, 1f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(.5f);
    }

    private void logoFadeOut() {
        LeanTween.alpha(logo, 0f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(1f);
        LeanTween.alpha(text, 0f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(1f).setOnComplete(showEarphoneRecommend);
    }

    private void showEarphoneRecommend() {
        LeanTween.alpha(earphone, 1f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(.5f);
        LeanTween.alpha(earphoneText, 1f, 1f).setEase(LeanTweenType.easeOutQuart).setDelay(.5f).setOnComplete(GoToMenu).setDelay(2);
    }

    private void GoToMenu() {
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.MENU_SCENE);
        });
    }

    public void loadingSpin() {
        LeanTween.rotateZ(loading, 180f, 1f).setEaseOutElastic().setDelay(.3f).setOnComplete(() => {
            LeanTween.rotateZ(loading, 360f, 1f).setEaseOutElastic().setDelay(.3f).setOnComplete(loadingSpin);
        });
    }
}
