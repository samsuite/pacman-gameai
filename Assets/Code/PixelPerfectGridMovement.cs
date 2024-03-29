﻿using System.Collections;
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

    public enum movement_layer {
        pacman,
        ghost
    };

    public movement_layer my_movement_layer;

    public Direction current_direction = Direction.none;
    public Direction desired_direction = Direction.none;
    public float speed = 1f;
    Vector3 velocity = new Vector3();
    Vector3 unrounded_position = new Vector3();
    Vector3 rounded_position = new Vector3();

    public int grid_x;
    public int grid_y;

    void Start () {
		//Vertical offset to start in grid
		transform.position += Grid.tile_size * new Vector3(0f,-.5f,0);
        unrounded_position = transform.position;
        set_grid_pos();
    }

	void Update () {

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

        // wraparound
        unrounded_position.x = ((unrounded_position.x + (Grid.tile_size*Grid.grid_width*1.5f)) % (Grid.tile_size*Grid.grid_width)) - (Grid.tile_size*Grid.grid_width*0.5f);
        unrounded_position.y = ((unrounded_position.y + (Grid.tile_size*Grid.grid_height*1.5f)) % (Grid.tile_size*Grid.grid_height)) - (Grid.tile_size*Grid.grid_height*0.5f);
        

        // can switch directions along the same axis instantly
        switch (desired_direction) {

            case Direction.up:
                if (can_move_up() && current_direction == Direction.down) {
                    current_direction = Direction.up;
                    desired_direction = Direction.none;
                }
                break;

            case Direction.down:
                if (can_move_down() && current_direction == Direction.up) {
                    current_direction = Direction.down;
                    desired_direction = Direction.none;
                }
                break;

            case Direction.left:
                if (can_move_left() && current_direction == Direction.right) {
                    current_direction = Direction.left;
                    desired_direction = Direction.none;
                }
                break;

            case Direction.right:
                if (can_move_right() && current_direction == Direction.left) {
                    current_direction = Direction.right;
                    desired_direction = Direction.none;
                }
                break;

            default:
                break;

        }


        if (current_direction == Direction.none) {
            current_direction = desired_direction;
            desired_direction = Direction.none;
        }
        else {
            if (just_passed_center()) {
                switch (desired_direction) {

                    case Direction.up:
                        if (can_move_up()) {
                            current_direction = Direction.up;
                            desired_direction = Direction.none;
                        }
                        break;

                    case Direction.down:
                        if (can_move_down()) {
                            current_direction = Direction.down;
                            desired_direction = Direction.none;
                        }
                        break;

                    case Direction.left:
                        if (can_move_left()) {
                            current_direction = Direction.left;
                            desired_direction = Direction.none;
                        }
                        break;

                    case Direction.right:
                        if (can_move_right()) {
                            current_direction = Direction.right;
                            desired_direction = Direction.none;
                        }
                        break;

                    case Direction.none:
                        if (!can_continue_in_current_direction()) {
                            current_direction = Direction.none;
                            desired_direction = Direction.none;
                        }
                        break;

                    default:
                        break;

                }


                // stop moving if you hit something
                switch (current_direction) {
                    case Direction.up:
                        if (!can_move_up()) {
                            current_direction = Direction.none;
                            set_on_grid();
                        }
                        break;

                    case Direction.down:
                        if (!can_move_down()) {
                            current_direction = Direction.none;
                            set_on_grid();
                        }
                        break;

                    case Direction.left:
                        if (!can_move_left()) {
                            current_direction = Direction.none;
                            set_on_grid();
                        }
                        break;

                    case Direction.right:
                        if (!can_move_right()) {
                            current_direction = Direction.none;
                            set_on_grid();
                        }
                        break;

                    default:
                        break;
                }

            }

        }


        set_grid_pos();
        set_on_grid();
		
        rounded_position.x = Mathf.Round(unrounded_position.x * Core.global.ppu) / Core.global.ppu;
        rounded_position.y = Mathf.Round(unrounded_position.y * Core.global.ppu) / Core.global.ppu;
        rounded_position.z = unrounded_position.z;

        transform.position = rounded_position + new Vector3(Grid.tile_size*0.5f, Grid.tile_size*0.5f, 0f);
	}

    void set_grid_pos () {
        grid_x = Grid.global.GetGridX(unrounded_position.x);
        grid_y = Grid.global.GetGridY(unrounded_position.y);
    }

    void set_on_grid () {

        if (current_direction != Direction.left && current_direction != Direction.right) {
            unrounded_position.x = (grid_x)*Grid.tile_size - (Grid.grid_width*Grid.tile_size*0.5f);
        }
        if (current_direction != Direction.up && current_direction != Direction.down) {
            unrounded_position.y = (grid_y)*Grid.tile_size - (Grid.grid_height*Grid.tile_size*0.5f);
        }

    }


    // did i just pass the center of a grid tile?
    public bool just_passed_center () {

        Vector3 prev_unrounded_position = unrounded_position - (velocity*1.5f);

        float offset_grid_x = Grid.global.GetGridX(unrounded_position.x + (Grid.tile_size/2f));
        float offset_grid_y = Grid.global.GetGridY(unrounded_position.y + (Grid.tile_size/2f));

        float prev_offset_grid_x = Grid.global.GetGridX(prev_unrounded_position.x + (Grid.tile_size/2f));
        float prev_offset_grid_y = Grid.global.GetGridY(prev_unrounded_position.y + (Grid.tile_size/2f));

        return (offset_grid_x != prev_offset_grid_x) || (offset_grid_y != prev_offset_grid_y);
    }



    public bool can_move_up () {

        if (my_movement_layer == movement_layer.ghost) {

            // don't move into another ghost
            for (int i = 0; i < Grid.global.all_ghosts.Count; i++) {
                if (Grid.global.all_ghosts[i].movement.grid_x == grid_x && Grid.global.all_ghosts[i].movement.grid_y == grid_y+1) {
                    return false;
                }
            }

            
            if (grid_y + 1 >= Grid.grid_height) {
                return !Grid.global.BlocksGhost(grid_x, 0);
            }

            return !Grid.global.BlocksGhost(grid_x, grid_y+1);
        }
        else if (my_movement_layer == movement_layer.pacman) {

            if (grid_y + 1 >= Grid.grid_height) {
                return !Grid.global.BlocksPlayer(grid_x, 0);
            }

            return !Grid.global.BlocksPlayer(grid_x, grid_y+1);
        }

        return false;
    }

	public bool can_move_down () {
        if (my_movement_layer == movement_layer.ghost) {

            // don't move into another ghost
            for (int i = 0; i < Grid.global.all_ghosts.Count; i++) {
                if (Grid.global.all_ghosts[i].movement.grid_x == grid_x && Grid.global.all_ghosts[i].movement.grid_y == grid_y-1) {
                    return false;
                }
            }

            if (grid_y - 1 < 0) {
                return !Grid.global.BlocksGhost(grid_x, Grid.grid_height-1);
            }

            return !Grid.global.BlocksGhost(grid_x, grid_y-1);
        }
        else if (my_movement_layer == movement_layer.pacman) {

            if (grid_y - 1 < 0) {
                return !Grid.global.BlocksPlayer(grid_x, Grid.grid_height-1);
            }

            return !Grid.global.BlocksPlayer(grid_x, grid_y-1);
        }

        return false;
    }

	public bool can_move_left () {
        if (my_movement_layer == movement_layer.ghost) {

            // don't move into another ghost
            for (int i = 0; i < Grid.global.all_ghosts.Count; i++) {
                if (Grid.global.all_ghosts[i].movement.grid_x == grid_x-1 && Grid.global.all_ghosts[i].movement.grid_y == grid_y) {
                    return false;
                }
            }

            if (grid_x - 1 < 0) {
                return !Grid.global.BlocksGhost(Grid.grid_width-1, grid_y);
            }

            return !Grid.global.BlocksGhost(grid_x-1, grid_y);
        }
        else if (my_movement_layer == movement_layer.pacman) {

            if (grid_x - 1 < 0) {
                return !Grid.global.BlocksPlayer(Grid.grid_width-1, grid_y);
            }

            return !Grid.global.BlocksPlayer(grid_x-1, grid_y);
        }

        return false;
    }

	public bool can_move_right () {
        if (my_movement_layer == movement_layer.ghost) {

            // don't move into another ghost
            for (int i = 0; i < Grid.global.all_ghosts.Count; i++) {
                if (Grid.global.all_ghosts[i].movement.grid_x == grid_x+1 && Grid.global.all_ghosts[i].movement.grid_y == grid_y) {
                    return false;
                }
            }

            if (grid_x + 1 >= Grid.grid_width) {
                return !Grid.global.BlocksGhost(0, grid_y);
            }

            return !Grid.global.BlocksGhost(grid_x+1, grid_y);
        }
        else if (my_movement_layer == movement_layer.pacman) {

            if (grid_x + 1 >= Grid.grid_width) {
                return !Grid.global.BlocksPlayer(0, grid_y);
            }

            return !Grid.global.BlocksPlayer(grid_x+1, grid_y);
        }

        return false;
    }



    public bool can_continue_in_current_direction () {
        switch (current_direction) {

            case Direction.up:
                return can_move_up();

            case Direction.down:
                return can_move_down();

            case Direction.left:
                return can_move_left();

            case Direction.right:
                return can_move_right();

            default:
                return false;
        }
    }
}
