using UnityEngine;

public class Key : MonoBehaviour {

    public int linkCode = 0;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            Destroy(gameObject);
            EventManager.TriggerEvent(GamesEvents.LOCK_OPEN, linkCode);
        }
    }
}
