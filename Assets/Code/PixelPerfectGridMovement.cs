using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectGridMovement : MonoBehaviour {

    public enum Direction {
        up,
        down,
        left,
        right,
        none
    };

    public Direction current_direction = Direction.none;
    public Direction desired_direction = Direction.none;
    public float speed = 1f;
    Vector3 velocity = new Vector3();
    Vector3 unrounded_position = new Vector3();
    Vector3 rounded_position = new Vector3();

    void Start () {
        unrounded_position = transform.position;
    }

	void Update () {

        // TODO:
        // lock to grid
        // anytime we move to a new grid tile, check if we can move in our desired direction. if so, that becomes our current direction and desired direction becomes none
        // note: if desired direction is none, we only stop moving if we CAN'T keep moving in our current direction. eg "none" is only a valid direction if there's a wall ahead


        switch (current_direction) {

            case Direction.up:
                velocity.x = 0f;
                velocity.y = speed;
                break;

            case Direction.down:
                velocity.x = 0f;
                velocity.y = -speed;
                break;

            case Direction.left:
                velocity.x = -speed;
                velocity.y = 0;
                break;

            case Direction.right:
                velocity.x = speed;
                velocity.y = 0;
                break;

            case Direction.none:
                velocity.x = 0;
                velocity.y = 0;
                break;

            default:
                break;

        }

        unrounded_position += velocity;
		
        rounded_position.x = Mathf.Round(unrounded_position.x * Core.global.ppu) / Core.global.ppu;
        rounded_position.y = Mathf.Round(unrounded_position.y * Core.global.ppu) / Core.global.ppu;
        rounded_position.z = unrounded_position.z;

        transform.position = rounded_position;
	}
}
