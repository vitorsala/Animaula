using UnityEngine;
using System.Collections;


public class PlayerActionFeedback : MonoBehaviour {

	public static PlayerActionFeedback GetNewPlayerActionFeedback(Transform targetTransform = null){
		GameObject go = Instantiate(SharedResources.GetSharedInstance().playerActionFeedback);
		go.transform.position = Vector3.zero;
		if(targetTransform != null) {
			go.transform.SetParent(targetTransform);
		}
		return go.GetComponent<PlayerActionFeedback>();
	}

	public Animator controller;
    
	void Update () {
		if(controller.GetCurrentAnimatorStateInfo(0).IsName("EndState")) {
			if(transform.parent.gameObject.name == "TheEmptyOne") {
				Destroy(transform.parent.gameObject);
			}
			Destroy(gameObject);
		}
	}

	public void ShowCorrect(){
        AudioController.SharedInstance.PlaySoundEffect(SharedResources.GetSharedInstance().correctSound, 0, 0.3f);
		controller.SetInteger("state", 1);
	}

	public void ShowWrong() {
        AudioController.SharedInstance.PlaySoundEffect(SharedResources.GetSharedInstance().wrongSound, 0, 0.6f);
        controller.SetInteger("state", 2);
	}
}
