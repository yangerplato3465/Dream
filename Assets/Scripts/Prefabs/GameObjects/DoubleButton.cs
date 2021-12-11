using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleButton : MonoBehaviour {
    
    public int linkCode = 0;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            EventManager.TriggerEvent(GamesEvents.TRIGGER_BUTTON, linkCode);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.tag == "Player"){
            EventManager.TriggerEvent(GamesEvents.UNTRIGGER_BUTTON, linkCode);
        }
    }
}
