using UnityEngine;
using System.Collections;

public enum StudentStatus{
	Neutral, 		// 0
	Chaotic, 		// 1
	ChaoticBusy, 	// 2
	Searching, 		// 3
	Crying,			// 4
	NeutralImmune
}

public enum StudentType{
	Dolphin,
	Penguin,
	Monkey
}

public class Student : MonoBehaviour {
	
	//public InteractableObject holding;

	public StudentType type;

	public int bagunca = 0;
	public int threshold = 20; // Quanto de bagunca precisa pro cara ficar mau
	public float activation = 1; // In Seconds

	public StudentStatus status = StudentStatus.Neutral;
	public Vector2 positionInClass;

	public Desk myDesk;

    public float timer = 0;

    public GameObject particles;

	public Animator plusOne;
    private Animator animator;
    private float currentTime = 0;
    private bool onDelay = true;

    // Use this for initialization
    void Awake () {
		status = StudentStatus.Neutral;

        LevelManager lm = LevelManager.sharedInstance;
        bagunca = 0;
	}

    void Start() {
        animator = GetComponent<Animator>();
        animator.SetInteger("type", (int)type);
    }

	void Update(){
		currentTime += Time.deltaTime;

        if(onDelay) {
            if(currentTime >= 3) onDelay = false;
            else return;
        }

        animator.SetInteger("status", (int)status);
        timer -= Time.deltaTime;

        switch(status) {
            case StudentStatus.Neutral:
                particles.SetActive(false);
                if(bagunca >= threshold) {
                    BecomeChaotic();
                }

                break;
            case StudentStatus.Searching:
                particles.SetActive(true);
                if(timer <= 0) {
                    particles.SetActive(false);
                    status = StudentStatus.Crying;
                    GameManager.GetSharedInstance().chaos++;
					showWrongMark();
                    myDesk.HideObject();
                }
                
                break;
            case StudentStatus.Crying:
                if(currentTime >= activation) {
                    currentTime = activation/2;
                    bagunca++;
                    Influences();
                }
                if(bagunca >= threshold) {
                    GameManager.GetSharedInstance().chaos--;
                    BecomeChaotic();
                }
                break;
            case StudentStatus.Chaotic:
                if(timer <= 0) {
                    Steal();
                    currentTime = 0;
                }
                break;
            case StudentStatus.ChaoticBusy:
                if(currentTime >= activation) {
                    currentTime = 0;
                    Influences();
                }
                if(timer <= 0) {
					status = StudentStatus.Chaotic;
					showWrongMark();
                    myDesk.objectInPlace.ResetPosition();
                    myDesk.HideObject();
                    timer = GameManager.GetSharedInstance().stealInTime;
                }
                break;
			case StudentStatus.NeutralImmune:
				if(timer <= 0) {
					status = StudentStatus.Neutral;
				}
				break;
        }
	}

	void showWrongMark(){
		PlayerActionFeedback tick1 = PlayerActionFeedback.GetNewPlayerActionFeedback(myDesk.transform);
		tick1.transform.localScale = new Vector3(0.2f, 0.2f, 1);
		tick1.ShowWrong();
	}

    void BecomeChaotic() {
        status = StudentStatus.Chaotic;
        myDesk.HideObject();
        timer = GameManager.GetSharedInstance().stealInTime;
        GameManager.GetSharedInstance().chaos++;
    }

    void Influences(){
		TileMap map = TileMap.GetSharedInstance();

		Vector2[] positions = new Vector2[]{
			positionInClass + Vector2.left * 2,
			positionInClass + Vector2.down * 2,
			positionInClass + Vector2.right * 2,
			positionInClass + Vector2.up * 2,
		};

		TileData td;
		foreach(Vector2 pos in positions) {
            if(pos.x >= 0 && pos.x < map.size.x && pos.y >= 0 && pos.y < map.size.y) {
                td = map.GetTileData(pos);
                if(td.property == TileProperties.Student
                    && td.reference.status == StudentStatus.Neutral) {
                    td.reference.bagunca++;
                }
            }
		}
	}

	void Steal(){
		TileMap map = TileMap.GetSharedInstance();
		Student[] possibleTargets = map.GetValidStudents();
		if(possibleTargets.Length > 0) {
			
			GameObject puff = Instantiate(SharedResources.GetSharedInstance().puff);

			Student target = possibleTargets[Random.Range(0, possibleTargets.Length - 1)];
			target.status = StudentStatus.Searching;
			target.timer = GameManager.GetSharedInstance().timeSeeking;

			myDesk.objectInPlace.ChangeTexture(target.myDesk.objectInPlace.GetTexture());
			myDesk.ShowObject();
			myDesk.objectInPlace.tag = "Stolen";
			myDesk.objectInPlace.SetAlpha(1f);

			puff.transform.position = myDesk.objectInPlace.transform.position;

			target.myDesk.ShowObject(true);

			myDesk.objectInPlace.target = target.myDesk.objectInPlace.gameObject;

			this.status = StudentStatus.ChaoticBusy;
			this.timer = target.timer;

        }
		else {
			this.timer = 0.5f;
		}
	}
}
