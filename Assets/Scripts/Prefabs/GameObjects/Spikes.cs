using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    
    public bool isMoving = false;
    public float time = 1f;
    public float delayTime = .5f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private Vector3 velocity = Vector3.zero;
    private bool toEnd = true;

    private void Start() {
        // startPoint = transform.position;
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
        if(isMoving) moveToEnd();
    }

    private void moveToEnd() {
        LeanTween.move(gameObject, endPoint, time).setOnComplete(moveToStart).setEaseOutCubic().setDelay(delayTime);
    }

    private void moveToStart() {
        LeanTween.move(gameObject, startPoint, time).setOnComplete(moveToEnd).setEaseOutCubic().setDelay(delayTime);
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }

}
