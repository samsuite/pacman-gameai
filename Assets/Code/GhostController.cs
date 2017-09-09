using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PixelPerfectGridMovement))]
public class GhostController : MonoBehaviour {

	public Texture2D[] eyes;

	PixelPerfectGridMovement movement;
	Material mat;
	PixelPerfectGridMovement.Direction lastDir;

	void Start () {
		movement = GetComponent<PixelPerfectGridMovement>();
		mat = GetComponent<MeshRenderer>().material;
	}

	void Update () {
		
		if(movement.just_passed_center() || movement.current_direction == PixelPerfectGridMovement.Direction.none){
			
			//Make list of possible moves
			var moveList = new List<PixelPerfectGridMovement.Direction>();
			if(movement.can_move_up()){
				moveList.Add(PixelPerfectGridMovement.Direction.up);
			}
			if(movement.can_move_down()){
				moveList.Add(PixelPerfectGridMovement.Direction.down);
			}
			if(movement.can_move_left()){
				moveList.Add(PixelPerfectGridMovement.Direction.left);
			}
			if(movement.can_move_right()){
				moveList.Add(PixelPerfectGridMovement.Direction.right);
			}
			//Remove backwards movement options, using last frame's direction
			switch (lastDir) {
				case PixelPerfectGridMovement.Direction.up:
					moveList.Remove(PixelPerfectGridMovement.Direction.down);
					break;

				case PixelPerfectGridMovement.Direction.down:
					moveList.Remove(PixelPerfectGridMovement.Direction.up);
					break;

				case PixelPerfectGridMovement.Direction.left:
					moveList.Remove(PixelPerfectGridMovement.Direction.right);
					break;

				case PixelPerfectGridMovement.Direction.right:
					moveList.Remove(PixelPerfectGridMovement.Direction.left);
					break;
			
				default:
					break;
			}

			//Pick random direction from remaining possible directions
			movement.desired_direction = moveList[UnityEngine.Random.Range(0,moveList.Count)];

		}

		if(movement.current_direction != PixelPerfectGridMovement.Direction.none){
			mat.SetTexture("_Eyes", eyes[(int)movement.current_direction]);
		}
		lastDir = movement.current_direction;

	}
}
