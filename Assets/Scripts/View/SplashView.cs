using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashView : MonoBehaviour {

    public RectTransform logo;
    public RectTransform text;
    void Start() {
        LeanTween.alpha(logo, 1f, .5f).setEase(LeanTweenType.easeOutQuart).setOnComplete(textfade);
        LeanTween.scale(logo.gameObject, Vector3.one * 1.5f, 2).setEasePunch();
    }

    private void textfade() {
        LeanTween.alpha(text, 1f, .5f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.scale(text.gameObject, Vector3.one * 1.5f, 2).setEasePunch();
    }
}
