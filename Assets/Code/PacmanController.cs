using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PixelPerfectGridMovement))]
public class PacmanController : MonoBehaviour {

    PixelPerfectGridMovement movement;
    public Texture2D[] sprites;
    Material mat;
    int current_frame = 0;
    float animation_timer = 0f;
    float frame_length = 0.1f;  // in seconds

	void Start () {
        mat = GetComponent<MeshRenderer>().material;

        movement = GetComponent<PixelPerfectGridMovement>();
		movement.current_direction = PixelPerfectGridMovement.Direction.none;
	}
	
	void Update () {

        // let's just arbitrarily say horizontal movement has priority over vertical
		if (Input.GetAxis("Horizontal") > 0) {
            movement.desired_direction = PixelPerfectGridMovement.Direction.right;
        }
        else if (Input.GetAxis("Horizontal") < 0) {
            movement.desired_direction = PixelPerfectGridMovement.Direction.left;
        }
        else if (Input.GetAxis("Vertical") > 0) {
            movement.desired_direction = PixelPerfectGridMovement.Direction.up;
        }
        else if (Input.GetAxis("Vertical") < 0) {
            movement.desired_direction = PixelPerfectGridMovement.Direction.down;
        }


        // only animate while pacman is moving
        if (movement.current_direction != PixelPerfectGridMovement.Direction.none) {
            animation_timer += Time.deltaTime;
            if (animation_timer > frame_length) {
                animation_timer -= frame_length;

                current_frame = (current_frame+1)%sprites.Length;
            }
        }


        mat.SetTexture("_MainTex", sprites[current_frame]);

        if (movement.current_direction == PixelPerfectGridMovement.Direction.right) {
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if (movement.current_direction == PixelPerfectGridMovement.Direction.up) {
            transform.eulerAngles = new Vector3(0f,0f,90f);
        }
        else if (movement.current_direction == PixelPerfectGridMovement.Direction.left) {
            transform.eulerAngles = new Vector3(0f,0f,180f);
        }
        else if (movement.current_direction == PixelPerfectGridMovement.Direction.down) {
            transform.eulerAngles = new Vector3(0f,0f,270f);
        }
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if(other.tag == "Dot"){
			//Award points, play noise

			//Remove Dot
			Destroy(other.gameObject);
		}
	}



	void OnCollisionEnter2D(Collision2D coll) {
		//Die and then restart
		Debug.Log("hit");

		//Die and then reload map
		Grid.global.ReloadMap();
	}
}
