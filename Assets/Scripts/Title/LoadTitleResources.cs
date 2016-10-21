using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadTitleResources : MonoBehaviour {

    public AudioClip introAudio;
	public Text highScoreText;

	void Awake() {
		int hs = 0;
		if(!PlayerPrefs.HasKey(Constants.HIGH_SCORE_KEY)) {
			PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, 0);
		}
		else {
			hs = PlayerPrefs.GetInt("highscore");
		}
		highScoreText.text = hs.ToString();
	}

	// Use this for initialization
	void Start () {
        AudioController.SharedInstance.ChangeMusic(introAudio);
		AudioController.SharedInstance.ChangeBGMVolume(1f);
	}
}
