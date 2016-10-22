using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StudentObject : MonoBehaviour {
    
    public GameObject target;

    public Student owner;
    public Vector3 originalPlace = Vector3.zero;

	private Vector3 originalScale;

    void Start() {
        originalScale = transform.localScale;
    }
    
    public void SetAlpha(float value) {
        SpriteRenderer objSR = gameObject.GetComponent<SpriteRenderer>();
        objSR.color = new Color(1, 1, 1, value);
    }

    public void Grow() {
        gameObject.transform.localScale = originalScale * 1.8f;
    }

    public void ResetSize() {
        gameObject.transform.localScale = originalScale;
    }

    public void ResetPosition() {
        transform.localPosition = originalPlace;
    }

    public void ChangeTexture(int index) {
        Texture2D texture = SharedResources.GetSharedInstance().itens[index];
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
