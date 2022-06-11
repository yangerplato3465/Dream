using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
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
    public GameObject emoji;
    private Vector2 moveInput;
    private bool canMove = true; 
    private Joystick joystick;


    private void Awake() {
        SetSkin();
    }

    void Start() {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
        if (isReverse) showEmoji();
    }

    void Update() {
        if(!canMove) return;
        int horizontal = 0;
        int vertical = 0;
        if(Mathf.Abs(joystick.Horizontal) > Mathf.Abs(joystick.Vertical)){
            vertical = 0;
            if(joystick.Horizontal > 0)horizontal = 1;
            else horizontal = -1;
        } else if(Mathf.Abs(joystick.Horizontal) < Mathf.Abs(joystick.Vertical)) {
            horizontal = 0;
            if(joystick.Vertical > 0)vertical = 1;
            else vertical = -1;
        }
        moveInput.x = horizontal;
        moveInput.y = vertical;

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));

        moveInput.Normalize();

        if(!isReverse) body.velocity = moveInput * moveSpeed;
        else body.velocity = -moveInput * moveSpeed;
        if(body.velocity.x > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            emoji.transform.localScale = new Vector3(-1, 1, 1);
        }
        if(body.velocity.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
            emoji.transform.localScale = new Vector3(1, 1, 1);
        }
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
            case "Player":
                canMove = false;
                animator.SetFloat("Speed", 0);
                break;

            case "Spikes":
                canMove = false;
                CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
                animator.SetFloat("Speed", 0);
                FindObjectOfType<AudioManager>().Play(SoundConst.HIT_SPIKE);
                EventManager.TriggerEvent(SystemEvents.GAME_LOSE);
                break;
        }
    }

    private void showEmoji() {
        emoji.SetActive(true);
        LeanTween.alpha(emoji, 1f, .8f);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
