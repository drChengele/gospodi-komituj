using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamCharacterController : MonoBehaviour {

    private float shipLateralSpeed = 5f;
    private float shipVerticalSpeed = 5f;
    private float baseShipForwardSpeed = 5f;
    private float currentBoosterSpeed = 0f;
    private float boosterMultiplayer = 1.1f;
    private float maxBoosterSpeed = 30f;
    private bool afterburnerActive = false;
    public Vector3 shipVelocity;

    //if inertia
    private float dampeningFactor;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        var horizontalSpeed = Input.GetAxis("Horizontal");
        var verticalSpeed = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump")) afterburnerActive = !afterburnerActive;

        AssignNewVeloctiy(horizontalSpeed * shipLateralSpeed,verticalSpeed * shipVerticalSpeed);
        MoveShip();
	}

    private void FixedUpdate()
    {
        if (afterburnerActive) KeepBoosting();
        else StopBoosting();
    }

    void KeepBoosting()
    {
        if (currentBoosterSpeed == 0f) currentBoosterSpeed = 0.1f;
        else
        {
            if (currentBoosterSpeed < maxBoosterSpeed + baseShipForwardSpeed) currentBoosterSpeed *= boosterMultiplayer;
            else currentBoosterSpeed = maxBoosterSpeed + baseShipForwardSpeed; //moze se izbaciti baseShipForwardSpeed da ima "treskanje" pri max brzini
        }
    }

    void StopBoosting()
    {
        if (currentBoosterSpeed != 0f)
        {
            if (currentBoosterSpeed < 0.005f) currentBoosterSpeed = 0f;
            else currentBoosterSpeed *= 1 / boosterMultiplayer;
        }
    }

    void AssignNewVeloctiy(float horizontal, float vertical)
    {
        shipVelocity = transform.forward * baseShipForwardSpeed + new Vector3(horizontal, vertical, currentBoosterSpeed);
    }

    void MoveShip()
    {
        transform.position += shipVelocity * Time.deltaTime;
    }
}
