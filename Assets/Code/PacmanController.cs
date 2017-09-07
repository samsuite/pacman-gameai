using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PixelPerfectGridMovement))]
public class PacmanController : MonoBehaviour {

    PixelPerfectGridMovement movement;

	void Start () {
        movement = GetComponent<PixelPerfectGridMovement>();
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
}
