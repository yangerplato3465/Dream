using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectView : MonoBehaviour {

    public Transform buttonLayout;
    public GameObject button;
    public GameObject circleSwipe;

    private string[] levelName;


    private void Awake() {
        LeanTween.moveX(circleSwipe, 0f, 0f);
        levelName = Directory.GetFiles(Application.dataPath + "/LevelData", "*.json");
        if(PlayerPrefs.GetInt(PlayerprefConst.CURRENT_AVAIL_LEVEL) == 0) PlayerPrefs.SetInt(PlayerprefConst.CURRENT_AVAIL_LEVEL, 1);
        int currentAvailableLevel = PlayerPrefs.GetInt(PlayerprefConst.CURRENT_AVAIL_LEVEL);
        
        for(int i = 0; i < levelName.Length; i++) {
            GameObject btn = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity, buttonLayout);
            int levelnum = i + 1;
            int gemNum = PlayerPrefs.GetInt("level" + levelnum);
            Button buttonComponent = btn.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnLevelButtonClick(levelnum));
            if (levelnum > currentAvailableLevel) buttonComponent.interactable = false;
            btn.GetComponent<LevelButton>().init(gemNum, levelnum);
        }
    }

    private void Start() {
        LeanTween.moveLocalX(circleSwipe, 3000f, 1f).setEaseOutQuad();
    }

    public void BackHome() {
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.MENU_SCENE);
        });
    }

    private void OnLevelButtonClick(int num){
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            Config.CURRENT_LEVEL = num;
            SceneManager.LoadScene(SceneConst.LEVELEDITOR_SCENE);
        });
    }
}
