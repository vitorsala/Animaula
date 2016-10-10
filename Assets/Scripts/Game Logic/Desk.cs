using UnityEngine;
using System.Collections;

public class Desk : MonoBehaviour {

	public GameObject[] objectList = new GameObject[1];
	public int selectedObject = 0;

	public Student deskOwner;
	public InteractableObject objectInPlace;

	// Use this for initialization
	void Start () {
		selectedObject = Random.Range(0, objectList.Length - 1);
		SpawnObject();
		objectInPlace.owner = deskOwner;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void HideObject(){
		objectInPlace.gameObject.SetActive(true);
		objectInPlace.gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	public void ShowObject(){
		objectInPlace.gameObject.SetActive(true);
	}

	void SpawnObject(){
		GameObject go = Instantiate(objectList[selectedObject]);
		go.transform.SetParent(gameObject.transform);
		go.transform.localPosition = new Vector3(0, 0.35f, 0);


		objectInPlace = go.GetComponent<InteractableObject>();
		objectInPlace.originalPlace = go.transform.localPosition;

		objectInPlace.gameObject.SetActive(false);
	}
}
