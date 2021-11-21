﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
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

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag)
        {
            case "Player":
                EventManager.TriggerEvent(GameEvents.GAME_WIN);
                break;
        }
    }
}
