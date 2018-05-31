using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCharacterController : MonoBehaviour {

    public float playerSpeed = 10;
    private Vector3 playerVelocity;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerVelocity = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"))) * playerSpeed * Time.deltaTime;
        transform.position += playerVelocity;
        playerVelocity = Vector3.zero;
	}
}
