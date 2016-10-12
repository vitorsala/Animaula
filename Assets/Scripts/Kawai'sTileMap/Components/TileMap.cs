using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Class to store a Tile Properties.
/// </summary>
public class TileMap : MonoBehaviour {
    private static TileMap sharedInstance;
    private TileMap() { }

    /// <summary>
    /// Tile Map's size.
    /// </summary>
	public Vector2 size = new Vector2(1,1);

    /// <summary>
    /// Individual tile's size.
    /// </summary>
	public Vector2 tileSize = new Vector2(1, 1);

    /// <summary>
    /// List of prefabs that will be used to build the map from editor.
    /// </summary>
    public GameObject[] prefabs = new GameObject[1];

    /// <summary>
    /// Matrix of tile information, this is only relevant if I'm going to do some pathfinding.
    /// </summary>
	[HideInInspector] public TileData[] tileInfo;
	[HideInInspector] public Vector2[] validTiles;
    /// <summary>
    /// Brush reference for editor.
    /// </summary>
#if UNITY_EDITOR
	[HideInInspector] public TileBrush brush;
#endif

    public static TileMap GetSharedInstance() {
        return sharedInstance;
    }

    /// <summary>
    /// Returns a tile's grid position based on it's scene position. <para /> 
    /// Note: Position may not be valid!
    /// </summary>
    /// <param name="pos">Position in scene</param>
    /// <returns>Index from a tile or an Vector2(-1,-1) if it is an invalid position.</returns>
	public Vector2 TileIndexForPos(Vector3 pos){
		Vector2 res = new Vector2();
		pos = (pos - transform.position + transform.position);
		res.x = Mathf.Floor(pos.x / tileSize.x);
		res.y = Mathf.Floor(pos.y / tileSize.y);
        if(res.x >= 0 && res.x < size.x && res.y >= 0 && res.y < size.y)
            return res;
        else
		    return new Vector2(-1,-1);
	}

    public Vector3 TilePosForIndex(Vector2 index) {
        if(index.x >= 0 && index.x < size.x && index.y >= 0 && index.y < size.y) {
            float x = (index.x * tileSize.x + tileSize.x / 2);
            float y = (index.y * tileSize.y + tileSize.y / 2);

            return new Vector3(x, y, 0);
        }
        else
            return new Vector3(-1, -1);
    }

    public TileData GetTileData(Vector2 v){
		return tileInfo [(int)(v.x + size.x * v.y)];
	}

	public TileData GetTileData(int x, int y) {
		return tileInfo [(int)(x + size.x * y)];
	}

	public Student[] GetValidStudents(){
		List<Student> validStudents = new List<Student>();
		foreach(Vector2 tileCoord in this.validTiles) {
			Student thisAluno = GetTileData(tileCoord).reference;
			if(thisAluno.status == StudentStatus.Neutral) {
				validStudents.Add(thisAluno);
			
			}
		}
		return validStudents.ToArray();

	}

	public void UpdateMatrix() {
		tileInfo = new TileData[(int)size.x * (int)size.y];
		List<Vector2> temp = new List<Vector2>();

		int j = 0;

		for(int i = 0; i < gameObject.transform.childCount; i++) {
			TileData go = gameObject.transform.GetChild(i).GetComponent<TileData>();
			if(go != null) {
				tileInfo[(int)(go.position.x + size.x * go.position.y)] = go;
				if(go.property == TileProperties.Student) {
					go.reference.positionInClass = go.position;

                    Desk sDesk = go.reference.myDesk;
                    sDesk.selectedObject = j;
                    sDesk.SpawnObject();

                    j++;
					temp.Add(go.position);
				}
			}
		}
		Student theChosenOne = GetTileData(temp[Random.Range(0, temp.Count - 1)]).reference;
		theChosenOne.bagunca = theChosenOne.threshold;
		theChosenOne.timer = 2;
		validTiles = temp.ToArray();
	}

    public void ChangeTile(int x, int y, GameObject newObject) {
        ChangeTile(new Vector2(x, y), newObject);
    }

    public void ChangeTile(Vector2 index, GameObject newObject) {
        // Don't allow to create an "void" tile in runtime.
        if(newObject != null) {
            GameObject go = GameObject.Find("tile_" + index.x + "," + index.y);
            Destroy(go);

            go = Instantiate(newObject);
            go.name = "tile_" + index.x + "," + index.y;
            go.transform.SetParent(this.transform.parent);
            go.transform.localPosition = TilePosForIndex(index);

            SpriteRenderer render = go.GetComponent<SpriteRenderer>();
            float sx = (size.x / render.sprite.bounds.size.x);
            float sy = (size.y / render.sprite.bounds.size.y);
            go.transform.localScale = new Vector3(sx, sy, 1);

            // Check if I have an Data on me. If not, I will adquire a new Data (Def: None).
            TileData td = go.GetComponent<TileData>();
            if(td == null) {
                td = go.AddComponent<TileData>();
                td.property = TileProperties.None;
            }
            td.position = index;
            tileInfo[(int)(index.x + size.x * index.y)] = td;
        }
    }

	public GameObject GetTileObject(int x, int y) {
		return GetTileObject(new Vector2(x, y));
	}


	public GameObject GetTileObject(Vector2 index){
		return GameObject.Find("tile_" + index.x + "," + index.y);
	}

    public void ClearAllTiles() {
        for(int i = 0; i < tileInfo.Length; i++) {
            tileInfo[i] = null;
        }
    }

    // Unity related
    void Start() {
        TileMap.sharedInstance = this;
        UpdateMatrix();
    }


#if UNITY_EDITOR
    // Tile map editor.
    void OnDrawGizmos() {
        Gizmos.color = Color.gray;
        for(var i = 0; i < size.x * size.y; i++) {
            var x = transform.position.x + (i % size.x) * tileSize.x + tileSize.x / 2;
            var y = transform.position.y + Mathf.Floor(i / size.x) * tileSize.y + tileSize.y / 2;
            Gizmos.DrawWireCube(new Vector3(x, y), tileSize);
        }

        Gizmos.color = Color.white;
        var tmp = new Vector3(size.x * tileSize.x, size.y * tileSize.y);
        Gizmos.DrawWireCube(transform.position + new Vector3(tmp.x / 2, tmp.y / 2), tmp);
    }

    public bool IsPrefabListComplete() {
        if(prefabs.Length == 0)
            return false;
        for(var i = 0; i < prefabs.Length; i++) {
            if(prefabs[i] == null)
                return false;
        }
        return true;
    }
#endif
}
