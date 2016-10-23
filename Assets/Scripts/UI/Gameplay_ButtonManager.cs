using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gameplay_ButtonManager : MonoBehaviour {

	public GameObject pauseMenu;

	private float time;
	private bool inPause = false;

    void OnApplicationFocus(bool hasFocus) {
        if(!hasFocus) {
            PauseButton();
        }
    }

    void OnApplicationPause(bool paused) {
        if(paused) {
            PauseButton();
        }
    }

    public void PauseButton() {
		if(!inPause) {
			time = Time.timeScale;
			Time.timeScale = 0f;
			pauseMenu.SetActive(true);
			inPause = true;
		}
	}

	public void ResumeButton() {
		inPause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = time;
	}

	public void HomeButton(string nome) {
		inPause = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		SceneManager.LoadScene(nome);
	}

    public void NextLevelButton() {
        LevelManager.sharedInstance.FinishLevel();
    }

    public void RestartButon() {
        LevelManager lm = LevelManager.sharedInstance;
        Time.timeScale = 1f;

        AudioController.SharedInstance.PlaySoundEffect(GameManager.GetSharedInstance().StartingSound, 0);
        Student[] students = GameManager.GetSharedInstance().GetStudents();
        foreach(Student s in students) {
            s.myDesk.objectInPlace.gameObject.SetActive(false);
            s.enabled = false;
        }
        lm.FinishLevel();
        lm.score = 0;
        lm.level = 0;
        pauseMenu.SetActive(false);
    }
}
