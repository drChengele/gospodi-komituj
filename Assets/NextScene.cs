using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire") || Input.GetMouseButton(0)) Application.LoadLevel(1);
	}
}
