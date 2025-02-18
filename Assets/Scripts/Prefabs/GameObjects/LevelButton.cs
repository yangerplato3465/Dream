using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public GameObject gem1;
    public GameObject gem2;
    public GameObject gem3;
    public Text text;

    public void init(int gemNumber, int levelNum) {
        text.text = levelNum.ToString();
        gem1.SetActive(gemNumber > 0);
        gem2.SetActive(gemNumber > 1);
        gem3.SetActive(gemNumber > 2);
    }
}
