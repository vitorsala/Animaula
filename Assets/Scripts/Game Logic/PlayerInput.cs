using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    private StudentObject draggingObject;

    private Vector3 touchPosWorld;
    private bool isDragging;

    // Use this for initialization
    void Start () {
        isDragging = false;
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
        // Se eu estou arrastando algo, esse algo tem que crescer e sair de baixo do meu dedo.
        if(isDragging && draggingObject != null) {
            draggingObject.Grow();
            draggingObject.gameObject.transform.position = new Vector3(touchPosWorld.x, touchPosWorld.y + 0.7f, 0);
        }
        // Mas ele tem que voltar a ser algo normal caso eu deixe de arrastar algo.
        else if(!isDragging && draggingObject != null) {
            draggingObject.ResetPosition();
            draggingObject.ResetSize();
            draggingObject = null;
        }
    }

    private void TouchesBegin(Vector3 position) {
        touchPosWorld = Camera.main.ScreenToWorldPoint(position);

        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        if(hitInformation.collider != null && hitInformation.transform.gameObject.tag == "Stolen") {
            isDragging = true;
            draggingObject = hitInformation.transform.gameObject.GetComponent<StudentObject>();
        }
    }

    private void TouchesMoved(Vector3 position) {
        touchPosWorld = Camera.main.ScreenToWorldPoint(position);
    }

    private void TouchesEnded(Vector3 position) {
        touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
        RaycastHit2D[] hitInformation = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

        bool isWrong = true;
        Vector3 posBuffer = draggingObject.gameObject.transform.position;
        // Verificação se eu soltei o objeto em cima do lugar que eu deveria soltar.
        foreach(RaycastHit2D hit in hitInformation) {
            if(draggingObject.target != null && hit.transform.gameObject == draggingObject.target) {
                StudentObject interactable = draggingObject.target.GetComponent<StudentObject>();
                interactable.owner.status = StudentStatus.Neutral;

                draggingObject.owner.status = StudentStatus.Chaotic;
                draggingObject.owner.timer = GameManager.GetSharedInstance().stealInTime;

                PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(interactable.owner.myDesk.transform);
                PlayerActionFeedback tick2 = PlayerActionFeedback.GetNewPlayerActionFeedback(draggingObject.owner.myDesk.transform);

                tick1.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                tick2.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                tick1.ShowCorrect();
                tick2.ShowCorrect();

                draggingObject.target.SetActive(false);
                draggingObject.gameObject.SetActive(false);

                isWrong = false;

            }
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

#if UNITY_EDITOR
    void ProcessInput() {
        if(Input.GetMouseButtonDown(0)) {
            TouchesBegin(Input.mousePosition);
        }

        if(Input.GetMouseButton(0) && isDragging) {
            TouchesMoved(Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(0) && isDragging) {
            TouchesEnded(Input.mousePosition);
        }
    }

#else
    void ProcessInput() {
        if(Input.touchCount > 0) {
            if(Input.GetTouch(0).phase == TouchPhase.Began) {
                TouchesBegin(Input.GetTouch(0).position);
            }

            if(Input.GetTouch(0).phase == TouchPhase.Moved && isDragging) {
                TouchesMoved(Input.GetTouch(0).position);
            }

            if(Input.GetTouch(0).phase == TouchPhase.Ended && isDragging) {
                TouchesEnded(Input.GetTouch(0).position);
            }
        }
    }
#endif

}
