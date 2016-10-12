using UnityEngine;
using System.Collections;

public class PuffComponent : MonoBehaviour {

    public AudioClip soundEffect;

    void Start() {
        AudioController.SharedInstance.PlaySoundEffect(soundEffect, 1);
    }
	
	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Puff-end")){
			Destroy(gameObject);
		}
	}
}
