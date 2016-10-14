using UnityEngine;
using System.Collections;

public class Desk : MonoBehaviour {

    public GameObject prefab;

	public int selectedObject = 0;

	public Student deskOwner;
	public InteractableObject objectInPlace;
    
	public void ShowObject(bool withQuestionMark = false){
		objectInPlace.gameObject.SetActive(true);
		objectInPlace.gameObject.transform.GetChild(0).gameObject.SetActive(withQuestionMark);
	}

	public void HideObject(){
		objectInPlace.gameObject.SetActive(false);
        objectInPlace.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

	public void SpawnObject(){
		GameObject go = Instantiate(prefab);
		go.transform.SetParent(gameObject.transform);
		go.transform.localPosition = new Vector3(0, 0.35f, 0);

		objectInPlace = go.GetComponent<InteractableObject>();
		objectInPlace.originalPlace = go.transform.localPosition;

        Texture2D texture = ItemReferences.GetSharedInstance().itens[selectedObject];
        Sprite item = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        SpriteRenderer objSR = objectInPlace.gameObject.GetComponent<SpriteRenderer>();
        objSR.sprite = item;

        objectInPlace.gameObject.SetActive(false);
	}
}
