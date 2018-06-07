using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMilageManager : MonoBehaviour {
    public float Mileage { get; private set; }
    public float goalMileage;
    private bool goalReached = false;

    private JamShipController jamShipController;
    [SerializeField] GameObject ship;

    // Use this for initialization
    void Start () {
        Mileage = 0;
        jamShipController = ship.GetComponent<JamShipController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (jamShipController == null) return;
        if (!goalReached)
        {
            Mileage += jamShipController.forwardSpeed * Time.deltaTime;

            UpdateStats();

            if (Mileage >= goalMileage) {
                Mileage = goalMileage;
                goalReached = true;
                ResolveGoalReached();
                Destroy(this); // ensure it only triggers once.
            }
        }
	}

    void UpdateStats() {
        GameManager.GlobalData.MetersCrossed = (int)Mileage;
        GameManager.GlobalData.Bounty = 13 * (int)Mileage;
    }

    public float remaining => goalMileage - Mileage;

    void ResolveGoalReached()
    {
        GameManager.GlobalData.IsGameOverAVictory = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
