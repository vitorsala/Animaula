using UnityEngine;
using System.Collections;

public class TimerBarComponent : MonoBehaviour {

	public float duration;
	public float barDisplay;
	private GameObject fill;
	private float xOffset;
	private float fillHeight;
	private float fillWidth;
	private float previousLength;

	void Start () {
		fill = transform.GetChild(0).gameObject;
		fillWidth = fill.GetComponent<Renderer>().bounds.max.x - fill.GetComponent<Renderer>().bounds.min.x;
		fillHeight = fill.GetComponent<Renderer>().bounds.max.y - fill.GetComponent<Renderer>().bounds.min.y;
		xOffset = -(fillWidth / 2) + 0.09f;
		fill.transform.position = new Vector3 (transform.position.x + xOffset, fill.transform.position.y, 0);
		fill.transform.localScale = new Vector3(0, 1, 1);
		previousLength = 0;
	}


	void Update() {
		float fillAmount = Time.deltaTime / duration;
		previousLength = previousLength + fillAmount;
		if(previousLength <= 1) {
			fill.transform.localScale += new Vector3(fillAmount, 0, 0);
			fill.transform.position = new Vector3(fill.transform.position.x + (fillAmount * fillWidth) / 2, fill.transform.position.y, 0);
		}
	}
}
