using UnityEngine;

public class Key : MonoBehaviour {

    public int linkCode = 0;

    private void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
        waveAnimation();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            Destroy(gameObject);
            EventManager.TriggerEvent(GamesEvents.LOCK_OPEN, linkCode);
        }
    }

    private void waveAnimation() {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        LeanTween.moveY(gameObject, yPos + 0.3f, 0.3f);
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
