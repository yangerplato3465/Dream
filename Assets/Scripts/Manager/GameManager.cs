using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Animator transition;
    public float transitionTime = .5f;

    // private void Awake() {
    //     DontDestroyOnLoad(this.gameObject);
    // }
    void Start() {
        AddListener();
    }

    private void onGameWin(object sender) {
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        Debug.Log("Game lose");
    }

    private void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    private void AddListener() {
        EventManager.AddListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.AddListener(SystemEvents.GAME_LOSE, onGameLose);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.RemoveListener(SystemEvents.GAME_LOSE, onGameLose);
    }
}
