using UnityEngine;
using System.Collections;

public class ItemReferences : MonoBehaviour {
	
	private static ItemReferences instance;
	public static ItemReferences GetSharedInstance(){
		return instance;
	}

	public Texture2D[] itens = new Texture2D[0];

	public GameObject puff;

	void Start(){
		instance = this;
	}
}
