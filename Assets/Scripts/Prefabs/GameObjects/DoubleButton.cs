using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleButton : MonoBehaviour {
    
    public int linkCode = 0;

    private void Start() {
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player"){ 
            FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
            EventManager.TriggerEvent(GamesEvents.TRIGGER_BUTTON, linkCode);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.tag == "Player"){
            FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
            EventManager.TriggerEvent(GamesEvents.UNTRIGGER_BUTTON, linkCode);
        }
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
