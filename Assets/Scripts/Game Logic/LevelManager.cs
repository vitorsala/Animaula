using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;
    public static LevelManager sharedInstance {
        get {
            return _instance;
        }
    }

    public int maxLevel = 9;
    public int level;
    public int score;

    private string sceneName;

    void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }

        sceneName = SceneManager.GetActiveScene().name;
        score = 0;
        level = 1;
    }

    void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	void Update () {
	    if(SceneManager.GetActiveScene().name != sceneName) {
            Destroy(this.gameObject);
        }
	}

    void NewLevel() {
        level++;
        SceneManager.LoadScene(sceneName);
    }

    public void FinishLevel() {
        FindObjectOfType<DoorComponent>().CloseDoor();
        score = GameManager.GetSharedInstance().score;
    }

    public void LevelAreReady() {
        GameManager.GetSharedInstance().score = score;
    }

    public void LevelAreClosed() {
        NewLevel();
    }
}
