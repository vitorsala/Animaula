using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StudentObject : MonoBehaviour {

    public bool isDragging = false;
    private Vector3 touchPosWorld;
    public GameObject target;

    public Student owner;
    public Vector3 originalPlace = Vector3.zero;


#if UNITY_EDITOR
    void Update() {
        if(Input.GetMouseButtonDown(0)) {

            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if(hitInformation.collider != null && hitInformation.transform.gameObject.tag == "Stolen" && hitInformation.transform.gameObject == this.gameObject) {
                isDragging = true;

            }
        }

        if(Input.GetMouseButton(0) && isDragging) {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(touchPosWorld.x, touchPosWorld.y, 0);
        }

        if(Input.GetMouseButtonUp(0) && isDragging) {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D[] hitInformation = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

            bool isWrong = true;
            Vector3 posBuffer = gameObject.transform.position;

            foreach(RaycastHit2D hit in hitInformation) {

                if(target != null && hit.transform.gameObject == target) {
                    StudentObject interactable = target.GetComponent<StudentObject>();
                    interactable.owner.status = StudentStatus.Neutral;
                    owner.status = StudentStatus.Chaotic;
                    owner.timer = GameManager.GetSharedInstance().stealInTime;

                    PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(interactable.owner.myDesk.transform);
                    PlayerActionFeedback tick2 = PlayerActionFeedback.GetNewPlayerActionFeedback(owner.myDesk.transform);

                    //						tick1.transform.localPosition = new Vector3(0, 0.4f, 0);
                    tick1.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                    //						tick2.transform.localPosition = new Vector3(0, 0.4f, 0);
                    tick2.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                    tick1.ShowCorrect();
                    tick2.ShowCorrect();

                    target.SetActive(false);
                    gameObject.SetActive(false);

                    isWrong = false;

                }

                gameObject.transform.localPosition = originalPlace;

            }

            if(isWrong) {
                GameObject go = new GameObject("TheEmptyOne");
                PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(go.transform);

                //						tick1.transform.position = transform.position;
                go.transform.position = new Vector3(posBuffer.x, posBuffer.y - 0.4f, 0);
                tick1.transform.localScale = new Vector3(0.09f, 0.09f, 1);

                tick1.ShowWrong();
            }

            isDragging = false;

        }
    }
#else
    void Update () {
        if(Input.touchCount > 0){

			if(Input.GetTouch(0).phase == TouchPhase.Began) {

				touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

				Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

				RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

				if(hitInformation.collider != null && hitInformation.transform.gameObject.tag == "Stolen" && hitInformation.transform.gameObject == this.gameObject) {
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

				bool isWrong = true;
				Vector3 posBuffer = gameObject.transform.position;

				foreach(RaycastHit2D hit in hitInformation) {
					
					if(target != null && hit.transform.gameObject == target) {
						StudentObject interactable = target.GetComponent<StudentObject>();
						interactable.owner.status = StudentStatus.Neutral;
						owner.status = StudentStatus.Chaotic;
						owner.timer = GameManager.GetSharedInstance().stealInTime;

						PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(interactable.owner.myDesk.transform);
						PlayerActionFeedback tick2 = PlayerActionFeedback.GetNewPlayerActionFeedback(owner.myDesk.transform);

//						tick1.transform.localPosition = new Vector3(0, 0.4f, 0);
						tick1.transform.localScale = new Vector3(0.2f, 0.2f, 1);

//						tick2.transform.localPosition = new Vector3(0, 0.4f, 0);
						tick2.transform.localScale = new Vector3(0.2f, 0.2f, 1);

						tick1.ShowCorrect();
						tick2.ShowCorrect();

						target.SetActive(false);
						gameObject.SetActive(false);

						isWrong = false;

					}

					gameObject.transform.localPosition = originalPlace;

				}

				if(isWrong) {
					GameObject go = new GameObject("TheEmptyOne");
					PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(go.transform);

					//						tick1.transform.position = transform.position;
					go.transform.position = new Vector3(posBuffer.x, posBuffer.y - 0.4f, 0);
					tick1.transform.localScale = new Vector3(0.09f, 0.09f, 1);

					tick1.ShowWrong();
				}

				isDragging = false;

			}
		}
	}
#endif
    public void SetAlpha(float value) {
        SpriteRenderer objSR = gameObject.GetComponent<SpriteRenderer>();
        objSR.color = new Color(1, 1, 1, value);
    }

    public void ResetPosition() {
        isDragging = false;
        transform.localPosition = originalPlace;

    }

    public void ChangeTexture(int index) {
        Texture2D texture = ItemReferences.GetSharedInstance().itens[index];
        Sprite item = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        SpriteRenderer objSR = gameObject.GetComponent<SpriteRenderer>();
        objSR.sprite = item;
    }
    
    public void ChangeTexture(Texture2D newTexture) {
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    }

    public Texture2D GetTexture() {
        return GetComponent<SpriteRenderer>().sprite.texture;
    }
}
