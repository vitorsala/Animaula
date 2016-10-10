using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InteractableObject : MonoBehaviour {
	
	public bool isDragging = false;
	private Vector3 touchPosWorld;
	public GameObject target;

	public Student owner;
	public Vector3 originalPlace = Vector3.zero;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount > 0){

			if(Input.GetTouch(0).phase == TouchPhase.Began) {

				touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

				Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

				RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

				if(hitInformation.collider != null && hitInformation.transform.gameObject == this.gameObject) {
					isDragging = true;

				}
			}

			if(Input.GetTouch(0).phase == TouchPhase.Moved && isDragging) {
				touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

				transform.position = new Vector3(touchPosWorld.x, touchPosWorld.y, 0);


			}

			if(Input.GetTouch(0).phase == TouchPhase.Ended && isDragging) {

				touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

				Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

				RaycastHit2D[] hitInformation = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

				foreach(RaycastHit2D hit in hitInformation) {
					
					if(target != null && hit.transform.gameObject == target) {
						target.GetComponent<InteractableObject>().owner.status = StudentStatus.Neutral;
						owner.status = StudentStatus.Chaotic;
						owner.timer = GameManager.GetSharedInstance().stealInTime;

						target.SetActive(false);
						gameObject.SetActive(false);

					}

					gameObject.transform.localPosition = originalPlace;

				}

				isDragging = false;

			}

		}
	}

	void CheckIfSelected(Touch t){

//		Vector2 mPos = t.position;
//		Ray2D ray = new Ray2D();
//		Ray ray = HandleUtility.GUIPointToWorldRay(mPos);
//		Vector2 pos = map.transform.InverseTransformPoint(ray.origin);
	}

	public void ChangeTexture(Texture2D newTexture){
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
	}

	public Texture2D GetTexture(){
		return GetComponent<SpriteRenderer>().sprite.texture;
	}


}
