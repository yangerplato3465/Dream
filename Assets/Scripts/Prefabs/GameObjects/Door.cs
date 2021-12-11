using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public int linkCode = 0;
    private int triggerCount = 0;
    void Start() {
        if(linkCode != 0) {
            EventManager.AddListener(GamesEvents.TRIGGER_BUTTON, onTriggered);
            EventManager.AddListener(GamesEvents.UNTRIGGER_BUTTON, onUntriggered);
        }
    }

    private void onTriggered(object sender) {
        if((int)sender == linkCode) {
            triggerCount++;
            if(triggerCount >= 2) doorOpen();
        }
    }

    private void onUntriggered(object sender) {
        if((int)sender == linkCode) {
            triggerCount--;
        }
    }

    private void doorOpen() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(GamesEvents.TRIGGER_BUTTON, onTriggered);
        EventManager.RemoveListener(GamesEvents.UNTRIGGER_BUTTON, onUntriggered);
    }
}
