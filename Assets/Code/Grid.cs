﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : Singleton<Grid> {

    public struct int_pair {
        public int x;
        public int y;
    }

	public TextAsset mapFile; //.csv or .tsv map file
	public GameObject wall, dot, gate, ghost, pacman, fruit;
	public char[,] tiles;

    public List<GhostController> all_ghosts = new List<GhostController>();

    public const float tile_size = 0.5f;
    public const int grid_width = 28;
    public const int grid_height = 31;
	public int dotsLeft = 0;

	private List<GameObject> spawnedObjects;

	void Awake() {

		FillGridData();
	}

	void Start () {
		spawnedObjects = new List<GameObject>();
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
				tiles[x, grid_height - y - 1] = tileKey;
			}
		}
	}

	public void ReloadMap(bool resetDots = true){
		DestroyMap(resetDots);
		SetupMap(resetDots);
	}

	void SetupMap(bool resetDots = true){
		//Map objects are loaded into world
		//Fits into debug grid
		Vector3 offset = new Vector3(- (grid_width - 1) * .5f * tile_size, - (grid_height - 1) * .5f * tile_size, 0);

		dotsLeft = 0;
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
				float depth = 0; //z depth of sprite (for having sprites go over walls)
				switch (tileKey) {

					case '0':
						
						break;


					case 'w':
						prefab = wall;
						break;


					case 'd':
						prefab = dot;
						name = "Dot";
						dotsLeft++;
						break;


					case 'p':
						//no powerups in this homework, defaults to dot
						prefab = dot;
						name = "Dot";
						dotsLeft++;
						break;

					case 'g':
						prefab = gate;
						break;

					case 's':
						prefab = ghost;
						depth = -1f;
						if(prefab != null){
							name = ghost.name;
						}
						break;

					case 'm':
						prefab = pacman;
						depth = -1f;
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

				if(prefab != null && (resetDots || name != "Dot")){
					//Gameobjects are attached to grid transform as a child
					GameObject go = (GameObject) Instantiate(prefab, new Vector3(tile_size * x, tile_size * y, depth) + offset, Quaternion.identity, transform);
					go.name = name;
					spawnedObjects.Add(go);
				}

			}	
		}

        // all ghosts have been spawned. go ahead and populate a list of them
        all_ghosts = new List<GhostController>(FindObjectsOfType(typeof(GhostController)) as GhostController[]);
	}

	void DestroyMap(bool resetDots = true){
		//Map must be destroyed before it is setup again (like when player dies and game restarts)
		List<GameObject> extraDots = new List<GameObject>();
		foreach(GameObject child in spawnedObjects){
			if(resetDots || child == null || child.tag != "Dot"){
				Destroy(child);
			}
			else{
				extraDots.Add(child);
			}
		}
		spawnedObjects.Clear();
		spawnedObjects.AddRange(extraDots);
	}

    public int GetGridX (float xpos) {
        return Mathf.RoundToInt((xpos+(grid_width*tile_size*0.5f))/Grid.tile_size);// + grid_width/2;
    }

    public int GetGridY (float ypos) {
        return Mathf.RoundToInt((ypos+(grid_height*tile_size*0.5f))/Grid.tile_size);// + grid_height/2;
    }

	public bool IsWall(int x, int y){
		return tiles[x,y] == 'w';
	}
	public bool IsGate(int x, int y){
		return tiles[x,y] == 'g';
	}
	public bool BlocksPlayer(int x, int y){
		return IsWall(x,y) || IsGate(x,y);
	}
	public bool BlocksGhost(int x, int y){
		return IsWall(x,y);
	}

}
