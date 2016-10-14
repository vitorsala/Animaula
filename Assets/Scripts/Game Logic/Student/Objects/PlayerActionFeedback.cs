using UnityEngine;
using System.Collections;


public class PlayerActionFeedback : MonoBehaviour {

	public static PlayerActionFeedback GetNewPlayerActionFeedback(Transform targetTransform = null){
		GameObject go = Instantiate(ItemReferences.GetSharedInstance().playerActionFeedback);
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
		controller.SetInteger("state", 1);
	}

	public void ShowWrong(){
		controller.SetInteger("state", 2);
	}
}
