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
            if (Mileage < goalMileage) Mileage += jamShipController.forwardSpeed * Time.deltaTime;
            else
            {
                Mileage = goalMileage;
                goalReached = true;
                ResolveGoalReached();
            }
        }
	}

    public float remaining => goalMileage - Mileage;

    void ResolveGoalReached()
    {
        GameManager.IsSuccessGameOver = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
