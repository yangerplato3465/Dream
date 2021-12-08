using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public int linkCode = 0;
    
    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.transform.tag)
        {
            case "Player":
                Destroy(gameObject);
                EventManager.TriggerEvent(GamesEvents.LOCK_OPEN, linkCode);
                break;
        }
    }
}
