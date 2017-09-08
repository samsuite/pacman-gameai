using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : Singleton<Grid> {

	public TextAsset mapFile; //.csv or .tsv map file
	public GameObject wall, dot, gate, ghost, pacman, fruit;
	public char[,] tiles;

    const float tile_size = 0.5f;
    const int grid_width = 28;
    const int grid_height = 31;

	private List<GameObject> spawnedObjects;

	void Awake() {

		FillGridData();
	}

	void Start () {
		SetupMap();
	}
	
	void Update () {

        Debug.DrawLine(new Vector3(tile_size*grid_width*-0.5f, tile_size*grid_height*0.5f, 0f), new Vector3(tile_size*grid_width*-0.5f, tile_size*grid_height*-0.5f, 0f), Color.red);
        Debug.DrawLine(new Vector3(tile_size*grid_width*-0.5f, tile_size*grid_height*-0.5f, 0f), new Vector3(tile_size*grid_width*0.5f, tile_size*grid_height*-0.5f, 0f), Color.red);
        Debug.DrawLine(new Vector3(tile_size*grid_width*0.5f, tile_size*grid_height*-0.5f, 0f), new Vector3(tile_size*grid_width*0.5f, tile_size*grid_height*0.5f, 0f), Color.red);
        Debug.DrawLine(new Vector3(tile_size*grid_width*0.5f, tile_size*grid_height*0.5f, 0f), new Vector3(tile_size*grid_width*-0.5f, tile_size*grid_height*0.5f, 0f), Color.red);
		
        for (int i = 1; i < grid_width; i++) {
            Vector3 top = new Vector3((tile_size*i)-(tile_size*grid_width*0.5f), tile_size*grid_height*0.5f, 0f);
            Vector3 bot = new Vector3((tile_size*i)-(tile_size*grid_width*0.5f), tile_size*grid_height*-0.5f, 0f);

            Debug.DrawLine(top, bot, Color.blue);
        }

        for (int j = 1; j < grid_height; j++) {
            Vector3 left = new Vector3(tile_size*grid_width*-0.5f, (tile_size*j)-(tile_size*grid_height*0.5f), 0f);
            Vector3 right = new Vector3(tile_size*grid_width*0.5f, (tile_size*j)-(tile_size*grid_height*0.5f), 0f);

            Debug.DrawLine(left, right, Color.blue);
        }

	}

	void OnDestroy(){
		DestroyMap();
	}


	void FillGridData(){
		//0,0 starts in the top left
		tiles = new char[grid_width , grid_height];

		//Break text up by lines first
		string[] rows = mapFile.text.Split(new string[]{"\n", "\r\n"}, System.StringSplitOptions.RemoveEmptyEntries);
		for (int y = 0; y < rows.Length; y++) {
			string line = rows[y];
			line.Trim();
			//Break text lines by , or tabs
			string[] tileChars = line.Split(new string[]{",","\t"}, System.StringSplitOptions.RemoveEmptyEntries);
			for (int x = 0; x < tileChars.Length; x++) {
				char tileKey;
				char.TryParse(tileChars[x], out tileKey);
				//If grid width or height is too small it will OutOfRangeException
				tiles[x,y] = tileKey;
			}
		}
	}

	public void ReloadMap(){
		DestroyMap();
		SetupMap();
	}

	void SetupMap(){
		//Map objects are loaded into world
		//Fits into debug grid
		Vector3 offset = new Vector3(- (grid_width - 1) * .5f * tile_size, (grid_height - 1) * .5f * tile_size, 0);
		spawnedObjects = new List<GameObject>();
		for(int y = 0; y < grid_height; y++){
			for(int x = 0; x < grid_width; x++){
				char tileKey = tiles[x,y];
				/*
					w	wall
					d	dot
					p	power pellet
					g	gate
					s	ghost spawn
					m	pac-man spawn
					f	fruit spawn
				 */

				GameObject prefab = null; //Prefab to be used to spawn
				string name = x + "," + y; //Name of new object to be called

				switch (tileKey) {

					case '0':
						
						break;


					case 'w':
						prefab = wall;
						break;


					case 'd':
						prefab = dot;		
						break;


					case 'p':
						//no powerups in this homework, defaults to dot
						prefab = dot;
						break;

					case 'g':
						prefab = gate;
						break;

					case 's':
						prefab = ghost;
						if(prefab != null){
							name = ghost.name;
						}
						break;

					case 'm':
						prefab = pacman;
						if(prefab != null){
							name = pacman.name;
						}
						break;

					case 'f':
						prefab = fruit;
						if(prefab != null){
							name = fruit.name;
						}
						break;

					default:
						Debug.LogError ("Unused char: " + tileKey);
						break;

				}

				if(prefab != null){
					//Gameobjects are attached to grid transform as a child
					GameObject go = (GameObject) Instantiate(prefab, new Vector3(tile_size * x, -tile_size * y, 0) + offset, Quaternion.identity, transform);
					go.name = name;
					spawnedObjects.Add(go);
				}

			}	
		}
	}

	void DestroyMap(){
		//Map must be destroyed before it is setup again (like when player dies and game restarts)
		foreach(GameObject child in spawnedObjects){
			Destroy(child);
		}
		spawnedObjects.Clear();
	}

	bool IsWall(int x, int y){
		return tiles[x,y] == 'w';
	}
	bool IsGate(int x, int y){
		return tiles[x,y] == 'g';
	}
	bool BlocksPlayer(int x, int y){
		return IsWall(x,y) || IsGate(x,y);
	}
	bool BlocksGhost(int x, int y){
		return IsWall(x,y);
	}

}
