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
        Light, Medium, Heavy, ChangingLevel, Finished
    }
    GameState state;

    public TimerBarComponent timerBar;

    public float classRoomTime;
    public int chaos;
    public int score;
    public float scoreTick;

    // Level Related
    private int childsToCorrupt;
    private float timeToCorrupt;
    private float corruptTimer;

    // General Parameters
    private float timer;
    private Student[] students;
    private float timeSinceLastTick;

    // Flow Parameters
    public int enterMediumState = 4;
    public int enterHeavyState = 8;
    public float timeSeeking = 0;
    public float stealInTime = 0;

    public int numberOfStudents = 12;

    // UI Control
    public GameObject finishedLevel;
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
        timer = classRoomTime + 3;
        timerBar.duration = timer;
        state = GameState.Light;
        chaos = 0;
        score = LevelManager.sharedInstance.score;
        scoreText.text = score.ToString();
        timeSinceLastTick = -2.9f;
        students = FindObjectsOfType<Student>();

        childsToCorrupt = (LevelManager.sharedInstance.level - 1) / 2;
        timeToCorrupt = timer / (childsToCorrupt + 1);
        corruptTimer = timeToCorrupt;

        bgAudioSource = AudioController.SharedInstance;

		bgAudioSource.ChangeMusic(lightBGM);
		bgAudioSource.ChangeBGMVolume(1f);
        bgAudioSource.PlaySoundEffect(StartingSound, 0);

        GameObject go = GameObject.Find("Class Number");
        Texture2D texture = SharedResources.GetSharedInstance().numbers[LevelManager.sharedInstance.level];
        Image im = go.GetComponent<Image>();
        SharedResources.GetSharedInstance();
        im.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
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
            LevelManager lm = LevelManager.sharedInstance;
            if(lm.level < lm.maxLevel) {
                state = GameState.ChangingLevel;
            }
            else {
                state = GameState.Finished;
                ganhou.SetActive(true);
            }
        }
        else { // Evita que o jogador continue ganhando pontos após o término do jogo
            timeSinceLastTick += Time.deltaTime;
            if(timeSinceLastTick >= scoreTick) {
                IncrementScore();
            }
            if(childsToCorrupt > 0) {
                corruptTimer -= Time.deltaTime;
                if(corruptTimer <= 0) {
                    int selected = Random.Range(0, students.Length - 1);
                    int iterator = selected;
                    while(students[iterator].status != StudentStatus.Neutral) {
                        iterator = (iterator + 1) % students.Length;
                        if(iterator == selected) break;
                    }
                    if(students[iterator].status == StudentStatus.Neutral) {
                        students[iterator].bagunca = students[iterator].threshold;
                    }
                    corruptTimer = timeToCorrupt;
                    childsToCorrupt--;
                }
            }
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
					bgAudioSource.ChangeMusic(heavyBGM, false);
					//bgAudioSource.ChangeBGMVolume(0.5f);
                    state = GameState.Heavy;
                }
                break;

            case GameState.Heavy:

                break;

            case GameState.ChangingLevel:
                if(students[0].enabled) {
                    bgAudioSource.PlaySoundEffect(StartingSound, 0);
                    foreach(Student s in students) {
                        s.myDesk.objectInPlace.gameObject.SetActive(false);
                        s.enabled = false;
                    }
                }
                finishedLevel.SetActive(true);
                //LevelManager.sharedInstance.FinishLevel();
                break;

		    case GameState.Finished:
			    Time.timeScale = 0f;

			    GameObject temp = (ganhou.activeSelf == true ? ganhou : perdeu);

			    Text finalScoreText = temp.transform.Find("score final").gameObject.GetComponent<Text>();
			    Text highScoreText = temp.transform.Find("Highscore value").gameObject.GetComponent<Text>();
			    Text recorde = temp.transform.Find("Recorde").gameObject.GetComponent<Text>();

			    finalScoreText.text = scoreText.text;
			    int hs = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY);

			    if(score > hs) { // Highscore
				    PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, score);
				    recorde.enabled = true;
				    hs = score;
			    }

			    highScoreText.text = hs.ToString();
                break;
        }
	}

    public Student[] GetStudents() {
        return students;
    }
}
