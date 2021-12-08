using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

    public int linkCode = 0;
    void Start() {
        if(linkCode != 0) {
            EventManager.AddListener(GamesEvents.LOCK_OPEN, onLockOpen);
        }
    }

    private void onLockOpen(object sender) {
        if((int)sender == linkCode) {
            Destroy(gameObject);
        }
    }
}
