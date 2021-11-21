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
        LoadNextLevel();
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
        EventManager.AddListener(GameEvents.GAME_WIN, onGameWin);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(GameEvents.GAME_WIN, onGameWin);
    }
}
