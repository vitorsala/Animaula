using UnityEngine;
using System.Collections;

public class SharedResources : MonoBehaviour {
	
	private static SharedResources instance;
	public static SharedResources GetSharedInstance(){
		return instance;
	}

	public Texture2D[] itens = new Texture2D[0];

	public GameObject puff;

	public GameObject playerActionFeedback;

    public AudioClip correctSound;
    public AudioClip wrongSound;

	void Start(){
		instance = this;
	}
}
