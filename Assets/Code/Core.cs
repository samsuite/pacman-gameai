﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Core : Singleton<Core> {

    public int ppu = 16;
	public int extra_lives = 2;
    [HideInInspector]
    public Camera main_cam;
    public int current_score = 0;
    public int high_score = 0;
    public float current_time = 0f;

    public Text current_score_text;
    public Text high_score_text;
    public Text time_text;

    void Awake () {
        main_cam = Camera.main;
        load_high_score();
    }

    void Update () {
        current_time += Time.deltaTime;
        time_text.text = current_time.ToString("F2");
        current_score_text.text = current_score.ToString("D4");
    }

    public void load_high_score () {
        if (PlayerPrefs.HasKey("high_score")) {
            high_score = PlayerPrefs.GetInt("high_score");
            high_score_text.text = high_score.ToString("D4");
        }
        else {
            high_score = 0;
        }
    }

    public void set_current_score_as_high_score () {
        high_score = current_score;
        PlayerPrefs.SetInt("high_score", high_score);
    }
	public void die () {
		if(extra_lives <= 0){
			out_of_lives();
		}
		else{
			Grid.global.ReloadMap(false);
			extra_lives--;
		}
	}
    void out_of_lives () {
        if (current_score > high_score) {
            set_current_score_as_high_score();
        }

		//PDF said that the button should Start/Restart the game
		/*
        current_score = 0;
        current_time = 0f;
        load_high_score();
        Grid.global.ReloadMap();
        */

		SceneManager.LoadScene("start");
    }

	public void next_level(){
		Grid.global.ReloadMap();
	}
}
