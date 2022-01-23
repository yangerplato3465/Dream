using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour {

    public RectTransform text;
    public float startTime = .5f;
    public float appearTime = 1f;
    public float duration = 5f;
    void Start() {
        text.LeanAlphaText(0f, 0f);
        Fadein();
    }

    private void Fadein() {
        text.LeanAlphaText(1f, appearTime).setDelay(startTime).setOnComplete(Fadeout);
    }

    private void Fadeout() {
        text.LeanAlphaText(0f, appearTime).setDelay(duration).setOnComplete(() => {Destroy(gameObject);});
    }
    
}
