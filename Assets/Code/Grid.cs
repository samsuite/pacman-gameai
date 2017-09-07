using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : Singleton<Grid> {

    public const float tile_size = 0.5f;
    public const int grid_width = 28;
    public const int grid_height = 31;

	void Start () {
		
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
}
