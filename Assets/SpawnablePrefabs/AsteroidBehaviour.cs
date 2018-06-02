using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

    private Vector3 rotationVector;
    private float rotationSpeed;
	// Use this for initialization
	void Awake () {
        rotationSpeed = Random.Range(1f, 100f);
        rotationVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
	}
	
	// Update is called once per frame
	void Update () {
        SelfSpinner();
	}

    void SelfSpinner()
    {
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }

    void KillMe()
    {
        Destroy(this.gameObject);
    }
}
