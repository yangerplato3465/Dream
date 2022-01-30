using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : MonoBehaviour {
    
    public void OnButtonClicked(string name) {
        Debug.Log(name);
        switch(name) {
            case "start":
                break;
            case "settings":
                break;
            case "twitter":
                Application.OpenURL(Config.TWITTER_URL);
                break;
            case "itch":
                Application.OpenURL(Config.ITCH_URL);
                break;
        }
    }
}
