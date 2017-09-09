using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PixelPerfectGridMovement))]
public class PacmanController : MonoBehaviour {

    PixelPerfectGridMovement movement;

	void Start () {
        movement = GetComponent<PixelPerfectGridMovement>();
		//Pacman always must move
		movement.current_direction = PixelPerfectGridMovement.Direction.left;
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
