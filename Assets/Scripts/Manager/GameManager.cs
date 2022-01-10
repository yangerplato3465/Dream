using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    public GameObject fade;

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
