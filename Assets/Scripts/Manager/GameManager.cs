using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    public RectTransform fade;
    public float fadeTime = 1f;
    public LeanTweenType fadeEaseType = LeanTweenType.easeInOutQuart;

    void Start() {
        AddListener();
    }

    private void setCurrentLevel(object level) {
        currentLevel = (int)level;
    }

    private void onGameWin(object sender) {
        fadein();
        Debug.Log("Game win");
    }

    private void onGameLose(object sender) {
        restartScene();
        Debug.Log("Game lose");
    }

    private void fadein() {
        LeanTween.alpha(fade, 1f, fadeTime).setEase(fadeEaseType).setOnComplete(fadeout);
    }

    private void fadeout() {
        loadLevel();
        LeanTween.alpha(fade, 0f, fadeTime).setEase(fadeEaseType);
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
