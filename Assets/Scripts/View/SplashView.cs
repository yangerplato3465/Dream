using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashView : MonoBehaviour {

    public RectTransform logo;
    public RectTransform text;
    public GameObject circleSwipe;

    void Start() {
        LeanTween.alpha(logo, 1f, .5f).setEase(LeanTweenType.easeOutQuart).setOnComplete(textfade);
        LeanTween.scale(logo.gameObject, Vector3.one * 1.5f, 2).setEasePunch();
    }

    private void textfade() {
        LeanTween.alpha(text, 1f, .5f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.scale(text.gameObject, Vector3.one * 1.5f, 2).setEasePunch().setOnComplete(GoToMenu);
    }

    private void GoToMenu() {
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.MENU_SCENE);
        });
    }
}
