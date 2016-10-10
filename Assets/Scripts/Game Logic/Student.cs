using UnityEngine;
using System.Collections;

public enum StudentStatus{
	Neutral, 		// 0
	Chaotic, 		// 1
	ChaoticBusy, 	// 2
	Searching, 		// 3
	Crying			// 4
}

public enum StudentType{
	Dolphin,
	Penguin,
	Monkey
}

public class Student : MonoBehaviour {
	
	public InteractableObject holding;

	public StudentType type;

	public int bagunca = 0;
	public int threshold = 20; // Quanto de bagunca precisa pro cara ficar mau
	public float activation = 1; // In Seconds

	public float timer = 0;

	public StudentStatus status = StudentStatus.Neutral;
	public Vector2 positionInClass;

	public Desk myDesk;

	private Animator animator;

	// Use this for initialization
	void Start () {
		status = StudentStatus.Neutral;
		bagunca = Random.Range(0, threshold - 5);
		animator = GetComponent<Animator>();
		animator.SetInteger("type", (int)type);
	}

	public float currentTime = 0;
	void Update(){
		currentTime += Time.deltaTime;
		animator.SetInteger("status", (int)status);
		if(timer >= 0) {
			timer -= Time.deltaTime;
			if(timer <= 0) {
				if(status == StudentStatus.Searching) {
					status = StudentStatus.Crying;
					myDesk.objectInPlace.gameObject.SetActive(false);
				}
				else if(status == StudentStatus.ChaoticBusy) {
					status = StudentStatus.Chaotic;
					myDesk.objectInPlace.gameObject.SetActive(false);
					timer = GameManager.GetSharedInstance().stealInTime;
				}
				else if(status == StudentStatus.Chaotic) {
					Steal();
				}
			}
		}

		if(currentTime >= activation){
			currentTime = 0;
			if(bagunca >= threshold && status != StudentStatus.Chaotic && status != StudentStatus.ChaoticBusy) { 
				status = StudentStatus.Chaotic;
				myDesk.objectInPlace.gameObject.SetActive(false);
				timer = GameManager.GetSharedInstance().stealInTime;
				GameManager.GetSharedInstance().chaos++;
				//gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0.6f, 1); // DEBUG
			}
			if(status == StudentStatus.ChaoticBusy) {
				Influences();
			}
			else if(status == StudentStatus.Crying) {
				//gameObject.GetComponent<SpriteRenderer>().color = new Color(0.58f, 0.62f, 0.85f, 1); // DEBUG
				bagunca++;
				Influences();
			}
			else if(status == StudentStatus.Searching) {
				//gameObject.GetComponent<SpriteRenderer>().color = new Color(0.57f, 0.85f, 0.59f, 1); // DEBUG

			}
		}

	}

	void Influences(){
		TileMap map = TileMap.GetSharedInstance();

		Vector2[] positions = new Vector2[]{
			positionInClass + Vector2.left * 2,
			positionInClass + Vector2.down,
			positionInClass + Vector2.right * 2,
			positionInClass + Vector2.up,
		};

		TileData td;
		foreach(Vector2 pos in positions) {
			td = map.GetTileData(pos);
			if(pos.x >= 0 && pos.x < map.size.x
				&& pos.y >= 0 && pos.y < map.size.y
				&& td.property == TileProperties.Student 
				&& td.reference.status == StudentStatus.Neutral) {

				td.reference.bagunca++;
			}
		}
	}

	void Steal(){
		TileMap map = TileMap.GetSharedInstance();
		Student[] possibleTargets = map.GetValidStudents();
		if(possibleTargets.Length > 0) {
			
			GameObject puff = Instantiate(ItemReferences.GetSharedInstance().puff);

			Student target = possibleTargets[Random.Range(0, possibleTargets.Length - 1)];
			target.status = StudentStatus.Searching;
			target.timer = GameManager.GetSharedInstance().timeSeeking;

			myDesk.objectInPlace.ChangeTexture(target.myDesk.objectInPlace.GetTexture());
			myDesk.ShowObject();
			puff.transform.position = myDesk.objectInPlace.transform.position;

			target.myDesk.HideObject();

			myDesk.objectInPlace.target = target.myDesk.objectInPlace.gameObject;

			this.status = StudentStatus.ChaoticBusy;
			this.timer = target.timer;
		}
	}
}
