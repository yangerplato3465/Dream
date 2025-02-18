using UnityEngine;

public class Lock : MonoBehaviour {

    public int linkCode = 0;
    void Start() {
        if(linkCode != 0) {
            EventManager.AddListener(GamesEvents.LOCK_OPEN, onLockOpen);
        }
        EventManager.AddListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }

    private void onLockOpen(object sender) {
        if((int)sender == linkCode) {
            Destroy(gameObject);
        }
    }

    private void destroySelf(object sender) {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(GamesEvents.LOCK_OPEN, onLockOpen);
        EventManager.RemoveListener(SystemEvents.DESTROY_FOR_LOADING, destroySelf);
    }
}
