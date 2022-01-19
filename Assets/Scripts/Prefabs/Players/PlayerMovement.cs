using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D body;
    private Vector2 moveInput;
    void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }

    void Update() {
        moveInput.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        moveInput.y = CrossPlatformInputManager.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));

        moveInput.Normalize();

        body.velocity = moveInput * moveSpeed;
    }

    private void destroySelf(object sender){
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag)
        {
            case "Player":
                EventManager.TriggerEvent(SystemEvents.GAME_WIN);
                break;
            
            case "Spikes":
                EventManager.TriggerEvent(SystemEvents.GAME_LOSE);
                break;
        }
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
