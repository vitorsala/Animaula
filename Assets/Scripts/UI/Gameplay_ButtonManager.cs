using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gameplay_ButtonManager : MonoBehaviour {

	public GameObject pauseMenu;

	private float time;
	private bool inPause = false;

	public void Button_Pause() {
		if(!inPause) {
			time = Time.timeScale;
			Time.timeScale = 0f;
			pauseMenu.SetActive(true);
			inPause = true;
		}
	}

	public void Button_Resume() {
		inPause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = time;
	}

	public void Button_Home(string nome) {
		inPause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		SceneManager.LoadScene(nome);
	}
}
