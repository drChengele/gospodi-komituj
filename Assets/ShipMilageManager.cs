﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMilageManager : MonoBehaviour {

    
    public float score;
    private float mileage;
    public float goalMileage;
    private bool goalReached = false;

    private JamShipController jamShipController;
    [SerializeField] GameObject ship;

    // Use this for initialization
    void Start () {
        jamShipController = ship.GetComponent<JamShipController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!goalReached)
        {
            if (mileage < goalMileage) mileage += jamShipController.forwardSpeed * Time.deltaTime;
            else
            {
                mileage = goalMileage;
                goalReached = true;
                ResolveGoalReached();
            }
        }
	}

    void ResolveGoalReached()
    {
        //kill ship movement cus i have the power.
        jamShipController.forwardSpeed = 0f;
    }
}
