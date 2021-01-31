using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Tilemaps;
using UnityEngine;



public class Map : MonoBehaviour {
	public static Map map;
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject firePrefab;
	[SerializeField] private GameObject matafuegoPrefab;
	[SerializeField] private GameObject victimaPrefab;
	//[SerializeField] private Material tileMaterial;
	//[SerializeField] private Material tileShadowMaterial;
	[SerializeField] private TextAsset tiledMapAsset;
	[SerializeField] private Sprite[] baseSprites;
	[SerializeField] private Texture interiorNormalMap;
	[SerializeField] private Material tileMaterial;
	private Material[] materials;


	private void Awake() {
		Map.map = this;

		TiledMap tiledMap = new TiledMap(tiledMapAsset.text, "Base");
		TiledMap elementsMap = new TiledMap(tiledMapAsset.text, "Elements");
		TiledMap colliderMap = new TiledMap(tiledMapAsset.text, "Collide");

		materials = new Material[baseSprites.Length];
		Vector2 textureSize = new Vector2(baseSprites[0].texture.width, baseSprites[0].texture.height);
		for (int i = 0; i < materials.Length; i++) {
			//materials[i] = new Material(Shader.Find("Standard"));
			//materials[i].SetTexture("_MainTex", baseSprites[i].texture);
			//materials[i].SetTexture("_BumpMap", interiorNormalMap);
			materials[i] = new Material(tileMaterial);
			Rect textureRect = baseSprites[i].textureRect;
			Rect normalizedTextureRect = new Rect(textureRect.x / textureSize.x, textureRect.y / textureSize.y, textureRect.width / textureSize.x, textureRect.height / textureSize.y);
			materials[i].mainTextureScale = new Vector2(normalizedTextureRect.width, normalizedTextureRect.height);
			materials[i].mainTextureOffset = new Vector2(normalizedTextureRect.x, normalizedTextureRect.y);
		}

		for (int j = 0; j < tiledMap.height; j++) {
			for (int i = 0; i < tiledMap.width; i++) {
				if (elementsMap.layer.tiles[i][tiledMap.height - 1 - j] == 204) {
					GameObject player = Instantiate(playerPrefab);
					player.transform.position = new Vector3(i + 0.5f, j + 0.5f, -1);
				}
				if (elementsMap.layer.tiles[i][tiledMap.height - 1 - j] == 189) {
					GameObject matafuego = Instantiate(matafuegoPrefab);
					matafuego.transform.position = new Vector3(i + 0.5f, j + 0.5f, -1);
				}
				if (elementsMap.layer.tiles[i][tiledMap.height - 1 - j] == 205) {
					GameObject fireA = Instantiate(firePrefab);
					fireA.transform.position = new Vector3(i + 0.5f, j + 0.5f, -1);
				}
				if (elementsMap.layer.tiles[i][tiledMap.height - 1 - j] == 173) {
					GameObject victima = Instantiate(victimaPrefab);
					victima.transform.position = new Vector3(i + 0.5f, j + 0.5f, -1);
				}

				if (tiledMap.layer.tiles[i][tiledMap.height - 1 - j] == 0) continue;
				GameObject tileGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Destroy(tileGO.GetComponent<BoxCollider>());
				tileGO.name = "tile " + i.ToString() + ", " + j.ToString();
				tileGO.transform.localEulerAngles = new Vector3(0, 0, 180);
				tileGO.transform.parent = transform;

				tileGO.GetComponent<Renderer>().material = materials[tiledMap.layer.tiles[i][tiledMap.height - 1 - j] - 1];
				if (colliderMap.layer.tiles[i][tiledMap.height - 1 - j] != 0) {
					tileGO.AddComponent<BoxCollider>();
					tileGO.transform.position = new Vector3(i + 0.5f, j + 0.5f, -1);
				}
				else {
					tileGO.transform.position = new Vector3(i + 0.5f, j + 0.5f, 0);
				}

			}

		}





		enabled = false;
	}




}
