using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using EZCameraShake;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed;
    public Animator animator;
    public Rigidbody2D body;
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
        if(Application.isEditor) {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        } else {
            moveInput.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            moveInput.y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.y));
        if(moveInput.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        if(moveInput.x < 0) transform.localScale = new Vector3(1, 1, 1);

        moveInput.Normalize();

        body.velocity = moveInput * moveSpeed;
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

    private void destroySelf(object sender){
        Debug.Log("PlayerdestroySelf");
        Destroy(gameObject);
    }

    private void footstep(){
        FindObjectOfType<AudioManager>().Play(SoundConst.FOOTSTEP);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        switch (other.transform.tag)
        {
            case "Player":
                if(!canMove) return;
                EventManager.TriggerEvent(SystemEvents.GAME_WIN);
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

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
