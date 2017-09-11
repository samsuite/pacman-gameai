using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(RawImage))]
public class LifeIndicator : MonoBehaviour {

	public int life = 1;
	RawImage image;
	// Use this for initialization
	void Start () {
		image = GetComponent<RawImage>();	
	}
	
	// Update is called once per frame
	void Update () {
		image.enabled = (Core.global.extra_lives >= life);
	}
}
