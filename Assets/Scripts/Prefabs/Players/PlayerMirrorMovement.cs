using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMirrorMovement : MonoBehaviour {
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D body;
    public bool isReverse = false;
    public RuntimeAnimatorController green;
    public RuntimeAnimatorController blue;
    public RuntimeAnimatorController red;
    public RuntimeAnimatorController orange;
    private Vector2 moveInput;
    private bool canMove = true; 

    private void Awake() {
        SetSkin();
    }
    void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }

    void Update() {
        if(!canMove) return;
        moveInput.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        moveInput.y = CrossPlatformInputManager.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));

        moveInput.Normalize();

        if(!isReverse) body.velocity = moveInput * moveSpeed;
        else body.velocity = -moveInput * moveSpeed;
        if(body.velocity.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        if(body.velocity.x < 0) transform.localScale = new Vector3(1, 1, 1);
    }

    private void SetSkin() {
        string skinInUse = PlayerPrefs.GetString(PlayerprefConst.ALIEN_IN_USE);
        switch (skinInUse)
        {
            case "green":
                animator.runtimeAnimatorController = green as RuntimeAnimatorController;
                break;

            case "blue":
                animator.runtimeAnimatorController = blue as RuntimeAnimatorController;
                break;

            case "red":
                animator.runtimeAnimatorController = red as RuntimeAnimatorController;
                break;

            case "orange":
                animator.runtimeAnimatorController = orange as RuntimeAnimatorController;
                break;
            
            default:
                animator.runtimeAnimatorController = green as RuntimeAnimatorController;
                break;
        }
    }

    private void footstep(){
        FindObjectOfType<AudioManager>().Play(SoundConst.FOOTSTEP);
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
