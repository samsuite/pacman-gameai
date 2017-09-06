using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core> {

    public int ppu = 16;
    [HideInInspector]
    public Camera main_cam;

    void Awake () {
        main_cam = Camera.main;
    }

}
