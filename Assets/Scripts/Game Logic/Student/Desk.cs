using UnityEngine;
using System.Collections;

public class Desk : MonoBehaviour {

    public GameObject prefab;

	public Student deskOwner;
	public StudentObject objectInPlace;
    
	public void ShowObject(bool withQuestionMark = false){
		objectInPlace.gameObject.SetActive(true);
		objectInPlace.gameObject.transform.GetChild(0).gameObject.SetActive(withQuestionMark);
	}

	public void HideObject(){
		objectInPlace.gameObject.SetActive(false);
        objectInPlace.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

	public void SpawnObject(int textureIndex){
		GameObject go = Instantiate(prefab);
		go.transform.SetParent(gameObject.transform);
		go.transform.localPosition = new Vector3(0, 0.35f, 0);

		objectInPlace = go.GetComponent<StudentObject>();
		objectInPlace.originalPlace = go.transform.localPosition;
        objectInPlace.ChangeTexture(textureIndex);
        objectInPlace.SetAlpha(0.4f);
//		objectInPlace.transform.localScale = new Vector3(0.2f, 0.2f, 1);

        objectInPlace.gameObject.SetActive(false);
	}
}
