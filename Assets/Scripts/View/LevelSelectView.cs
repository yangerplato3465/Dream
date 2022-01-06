using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectView : MonoBehaviour {

    public Transform buttonLayout;
    public GameObject button;

    private string[] levelName;


    private void Awake() {
        levelName = Directory.GetFiles(Application.dataPath + "/LevelData", "*.json");
        
        for(int i = 0; i < levelName.Length; i++) {
            GameObject btn = Instantiate(button, new Vector3(0, 0, 0), Quaternion.identity, buttonLayout);
            int levelnum = i + 1;
            btn.GetComponent<Button>().onClick.AddListener(() => OnLevelButtonClick(levelnum));
            btn.GetComponentInChildren<Text>().text = "Level " + (levelnum);
            btn.SetActive(true);
        }
    }

    private void OnLevelButtonClick(int num){
        Debug.Log("Clicked Level " + num);
    }
}
