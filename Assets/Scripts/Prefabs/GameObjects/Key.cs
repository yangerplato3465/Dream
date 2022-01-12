using UnityEngine;

public class Key : MonoBehaviour {

    public int linkCode = 0;

    private void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            Destroy(gameObject);
            EventManager.TriggerEvent(GamesEvents.LOCK_OPEN, linkCode);
        }
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
