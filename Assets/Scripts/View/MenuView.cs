using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuView : MonoBehaviour {

    public RectTransform shopPanel;
    public GameObject circleSwipe;

    private void Awake() {
        LeanTween.moveX(circleSwipe, 0f, 0f);
    }

    void Start() {
        Debug.Log("start functin");
        LeanTween.moveLocalX(circleSwipe, -2400f, 1f).setEaseOutQuad();
    }
    
    public void OnButtonClicked(string name) {
        Debug.Log(name);
        switch(name) {
            case "start":
                GoToLevelSelectScene();
                break;
            case "shop":
                OpenShopPanel();
                FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLICK);
                break;
            case "twitter":
                Application.OpenURL(Config.TWITTER_URL);
                break;
            case "itch":
                Application.OpenURL(Config.ITCH_URL);
                break;
            case "close":
                CloseShopPanel();
                FindObjectOfType<AudioManager>().Play(SoundConst.BUTTON_CLOSE);
                break;
        }
    }

    private void OpenShopPanel() {
        shopPanel.gameObject.SetActive(true);
        LeanTween.scale(shopPanel.gameObject, Vector3.one * 1.1f, .8f).setEasePunch();
    }

    private void CloseShopPanel() {
        LeanTween.scale(shopPanel.gameObject, Vector3.one * .6f, .3f).setEaseInBack().setOnComplete(() => {
            shopPanel.gameObject.SetActive(false);
            LeanTween.scale(shopPanel.gameObject, Vector3.one * 1f, 0);
        });
    }

    public void OnToggleSound(Toggle disable) {
        AudioManager.ToggleAllSFX(disable.isOn);
        Debug.Log("Sound disable is " + disable.isOn);
    }

    private void GoToLevelSelectScene() {
        LeanTween.moveX(circleSwipe, 0f, 1f).setEaseOutQuad().setOnComplete(() => {
            SceneManager.LoadScene(SceneConst.LEVELSELECT_SCENE);
        });
    }
}
