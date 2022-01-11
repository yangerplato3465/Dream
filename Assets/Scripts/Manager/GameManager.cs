using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    public GameObject fade;

    void Start() {
        AddListener();
    }

    private void setCurrentLevel(object level) {
        currentLevel = (int)level;
    }

    private void onGameWin(object sender) {
        loadLevel();
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        restartScene();
        Debug.Log("Game lose");
    }

    private void fadein() {
        LeanTween.alpha(fade, 1f, 1f).setOnComplete(fadeout);
    }

    private void fadeout() {
        LeanTween.alpha(fade, 0f, 1f);
    }

    private void restartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void loadLevel() {
        EventManager.TriggerEvent(SystemEvents.LOAD_LEVEL, currentLevel + 1);
    }

    private void AddListener() {
        EventManager.AddListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.AddListener(SystemEvents.GAME_LOSE, onGameLose);
        EventManager.AddListener(SystemEvents.SET_LEVEL, setCurrentLevel);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.GAME_WIN, onGameWin);
        EventManager.RemoveListener(SystemEvents.GAME_LOSE, onGameLose);
        EventManager.AddListener(SystemEvents.SET_LEVEL, setCurrentLevel);
    }
}
