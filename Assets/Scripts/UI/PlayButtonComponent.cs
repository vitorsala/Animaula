using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayButtonComponent : MonoBehaviour {

    public string levelSceneName;
    public string tutorialSceneName;

    public void IniciarJogo() {
        if(PlayerPrefs.HasKey("DidTutorial")) {
            SceneManager.LoadScene(levelSceneName);
        }
        else {
            PlayerPrefs.SetInt("DidTutorial", 0);
            SceneManager.LoadScene(tutorialSceneName);
        }
    }
}
