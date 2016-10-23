using UnityEngine;
using System.Collections;

public class SharedResources : MonoBehaviour {
	
	private static SharedResources instance;
	public static SharedResources GetSharedInstance(){
		return instance;
	}
    void Awake() {
        instance = this;
    }

	public Texture2D[] itens = new Texture2D[0];
    public Texture2D[] numbers = new Texture2D[0];

    public GameObject puff;

	public GameObject playerActionFeedback;

    public AudioClip correctSound;
    public AudioClip wrongSound;

}
