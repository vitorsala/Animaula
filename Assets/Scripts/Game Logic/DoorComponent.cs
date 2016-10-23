using UnityEngine;
using System.Collections;

public class DoorComponent : MonoBehaviour {

    public AudioClip openDoorSFX;
    public AudioClip closeDoorSFX;

    Animator animator;
    bool didSentOpenMsg = false;
    bool didSentCloseMsg = false;

    void Awake() {
        Vector3 scale = gameObject.transform.GetChild(0).localScale;
        scale.x = (Camera.main.orthographicSize * 2.0f * ((float)Screen.width / (float)Screen.height)) / 10;
        scale.x += 0.1f;
        gameObject.transform.GetChild(0).localScale = scale;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        AudioController.SharedInstance.PlaySoundEffect(openDoorSFX, 2);
        animator.SetTrigger("Open");
	}
	
    public void CloseDoor() {
        AudioController.SharedInstance.PlaySoundEffect(closeDoorSFX, 2);
        animator.SetTrigger("Close");
    }

	// Update is called once per frame
	void Update () {
        if(!didSentOpenMsg && animator.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
            LevelManager.sharedInstance.LevelAreReady();
            didSentOpenMsg = true;
        }
        if(didSentOpenMsg && !didSentCloseMsg && animator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) {
            LevelManager.sharedInstance.LevelAreClosed();
            didSentCloseMsg = true;
        }
	}
}
