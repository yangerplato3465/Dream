using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Animator transition;
    public float transitionTime = .5f;
    private int currentLevel = 0;

    void Start() {
        AddListener();
    }

    private void onGameWin(object sender) {
        loadLevel();
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        restartScene();
        Debug.Log("Game lose");
    }

    private void restartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void loadLevel() {
        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel);
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
