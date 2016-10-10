using UnityEngine;
using System.Collections;

public class DestroyAfterAnimation : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Puff-end")){
			Destroy(gameObject);
		}
	}
}
