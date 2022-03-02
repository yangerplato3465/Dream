using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMirrorMovement : MonoBehaviour {
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D body;
    private Vector2 moveInput;
    private bool canMove = true; 
    void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }

    void Update() {
        if(!canMove) return;
        moveInput.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        moveInput.y = CrossPlatformInputManager.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));
        if(moveInput.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        if(moveInput.x < 0) transform.localScale = new Vector3(1, 1, 1);

        moveInput.Normalize();

        body.velocity = moveInput * moveSpeed;
    }

    private void destroySelf(object sender){
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag)
        {
            case "Spikes":
                canMove = false;
                EventManager.TriggerEvent(SystemEvents.GAME_LOSE);
                break;
        }
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
