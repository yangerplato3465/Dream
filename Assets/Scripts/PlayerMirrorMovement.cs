using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorMovement : MonoBehaviour {
    public float moveSpeed;
    public Rigidbody2D body;
    private Vector2 moveInput;
    void Start() {
    }

    void Update() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        body.velocity = moveInput * moveSpeed;
    }
}
