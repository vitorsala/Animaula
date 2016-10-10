using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	private GameManager(){}

	public static GameManager GetSharedInstance(){
		return instance;
	}

	Student[] studentInClass;
	int qtdPerCol;

	public TimerBarComponent timerBar;

	public float timer;
	public int chaos;

	public float timeCrying = 0;
	public float timeSeeking = 0;
	public float stealInTime = 0; 
	public int numberOfStudents = 12;

	public bool paused = false;

	public GameObject ganhou;
	public GameObject perdeu;

	// Audios
	public AudioClip normal;
	public AudioClip medio;
	public AudioClip alto;

	private bool podeEntrarNoHeavy = true;
	private bool podeEntrarNoMedio = true;

	// Camera reference;
	AudioSource bgMusic;


	// Use this for initialization
	void Start () {
		instance = this;
		timerBar.duration = timer;
		chaos = 0;
		bgMusic = Camera.main.GetComponent<AudioSource>();
	}


	// Update is called once per frame
	void Update () {
		if(chaos >= 4 && podeEntrarNoMedio) {
			podeEntrarNoMedio = false;
			float time = bgMusic.time;
			bgMusic.clip = medio;
			bgMusic.Play();
			bgMusic.time = time;
		}

		if(chaos >= 8 && podeEntrarNoHeavy) {
			podeEntrarNoHeavy = false;
			bgMusic.volume = 0.5f;
			bgMusic.clip = alto;
			bgMusic.Play();
		}

		timer -= Time.deltaTime;
		if(chaos >= numberOfStudents) 
		{
			Time.timeScale = 0f;
			perdeu.SetActive(true);
			// PERDER
		}
		else if(chaos < numberOfStudents && timer <= 0) {
			// Vencer
			Time.timeScale = 0f;
			ganhou.SetActive(true);
		}
	}
}
