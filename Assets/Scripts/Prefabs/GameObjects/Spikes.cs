using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    
    public bool isMoving = false;
    public float smoothTime = 0.15f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private Vector3 velocity = Vector3.zero;
    private bool toEnd = true;

    private void Start() {
        startPoint = transform.position;
    }
    private void Update() {
        if(isMoving) {
            if(transform.position == startPoint) toEnd = true;
            if(transform.position == endPoint) toEnd = false;
            
            if(toEnd) transform.position = Vector3.SmoothDamp(transform.position, endPoint, ref velocity, smoothTime);
            else transform.position = Vector3.SmoothDamp(transform.position, startPoint, ref velocity, smoothTime);
        }
    }

}
