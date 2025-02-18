using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    private float top = 0f;
    private float bottom = 0f;
    private float randomDelay = 0f;
    void Start() {
        top = transform.position.y + 0.1f;
        bottom = transform.position.y - 0.1f;
        randomDelay = Random.Range(0f, 1f);
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
        LeanTween.moveY(gameObject, top, .8f).setOnComplete(FloatingDown).setEaseInOutSine().setDelay(randomDelay);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            Destroy(gameObject);
            EventManager.TriggerEvent(GamesEvents.COLLECT_GEM);
        }
    }

    private void FloatingUp() {
        LeanTween.moveY(gameObject, top, .8f).setOnComplete(FloatingDown).setEaseInOutSine();
    }

    private void FloatingDown() {
        LeanTween.moveY(gameObject, bottom, .8f).setOnComplete(FloatingUp).setEaseInOutSine();
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
