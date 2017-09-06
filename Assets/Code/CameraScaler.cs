using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scale the camera based on resolution so we never get any weird sprite scaling issues
[RequireComponent (typeof(Camera))]
public class CameraScaler : MonoBehaviour {

    Camera cam;
    const int scale_factor = 2;

	void Start () {
        cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height / (2f * Core.global.ppu * scale_factor);
	}
}
