using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorMovement : MonoBehaviour {
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D body;
    private Vector2 moveInput;
    void Start() {
    }

    void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));

        moveInput.Normalize();

        body.velocity = moveInput * moveSpeed;
    }
}
