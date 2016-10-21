using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private GameManager() { }

    public static GameManager GetSharedInstance() {
        return instance;
    }

    enum GameState {
        Light, Medium, Heavy, Finished
    }
    GameState state;

    public TimerBarComponent timerBar;

    public float timer;
    public int chaos;
    public int score;
    public float scoreTick;

    private Student[] students;
    private float timeSinceLastTick;

    // Flow Parameters
    public int enterMediumState = 4;
    public int enterHeavyState = 8;
    public float timeSeeking = 0;
    public float stealInTime = 0;

    public int numberOfStudents = 12;

    // UI Control
    public GameObject ganhou;
    public GameObject perdeu;
    public Text scoreText;

    // Audios
    public AudioClip lightBGM;
    public AudioClip mediumBGM;
    public AudioClip heavyBGM;

    // Sound Effects
    public AudioClip StartingSound;

    // Camera reference;
    private AudioController bgAudioSource;

    void Awake() {
        if(instance != null && instance != this) {
            Destroy(this);
            return;
        }
        instance = this;
    }

	// Use this for initialization
	void Start () {
        Time.timeScale = 1f;
        timerBar.duration = timer;
        state = GameState.Light;
        chaos = 0;
        score = 0;

        timeSinceLastTick = -2.9f;
        students = FindObjectsOfType<Student>();

		bgAudioSource = AudioController.SharedInstance;

		bgAudioSource.ChangeMusic(lightBGM);
		bgAudioSource.ChangeBGMVolume(1f);
        bgAudioSource.PlaySoundEffect(StartingSound, 0);
    }


	private void IncrementScore(){
		foreach(Student s in students) {
			if(s.status == StudentStatus.Neutral || s.status == StudentStatus.Searching) {
				s.plusOne.SetTrigger("TriggerAnimation");
				score++;
			}
		}
		//            score += numberOfStudents - chaos;
		scoreText.text = score.ToString();
		timeSinceLastTick = 0;
	}


	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if(chaos >= numberOfStudents) {
            state = GameState.Finished;
            perdeu.SetActive(true);
            // PERDER
        }
        else if(chaos < numberOfStudents && timer <= 0) {
            // Vencer
			//IncrementScore();
            state = GameState.Finished;
            ganhou.SetActive(true);
        }

        timeSinceLastTick += Time.deltaTime;
        if(timeSinceLastTick >= scoreTick) {
			IncrementScore();
        }

        switch(state) {
            case GameState.Light:
                if(chaos >= enterMediumState) {
                    bgAudioSource.ChangeMusic(mediumBGM, false);
                    state = GameState.Medium;
                }
                break;
            case GameState.Medium:
                if(chaos >= enterHeavyState) {
					bgAudioSource.ChangeMusic(heavyBGM);
					bgAudioSource.ChangeBGMVolume(0.5f);
                    state = GameState.Heavy;
                }
                break;

            case GameState.Heavy:

                break;

		case GameState.Finished:
			Time.timeScale = 0f;

			GameObject temp = (ganhou.activeSelf == true ? ganhou : perdeu);


			Text finalScoreText = temp.transform.Find("score final").gameObject.GetComponent<Text>();
			Text highScoreText = temp.transform.Find("Highscore value").gameObject.GetComponent<Text>();
			Text recorde = temp.transform.Find("Recorde").gameObject.GetComponent<Text>();

			finalScoreText.text = scoreText.text;
			int hs = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY);
			if(score > hs) {
				PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, score);
				recorde.enabled = true;
				hs = score;
			}

			highScoreText.text = hs.ToString();

            break;
        }
	}
}
