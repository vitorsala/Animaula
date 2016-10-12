using UnityEngine;
using System.Collections;

public class LoadTitleResources : MonoBehaviour {

    public AudioClip introAudio;

	// Use this for initialization
	void Start () {
        AudioController.SharedInstance.ChangeMusic(introAudio);
	}
}
