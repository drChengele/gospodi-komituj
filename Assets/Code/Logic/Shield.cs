using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ShipSystem {

    bool shieldActive = false;
    [SerializeField] PilotController pilotController;
    [SerializeField] GameObject shieldVisuals;
    [SerializeField] float minInvulnerabilityTime;
    float shieldUpTime;
    public bool IsInvulnerable { get; private set; }

    // Use this for initialization
    

    void Update()
    {
        if (shieldActive)
            KeepShieldUp();
        else
        {
            //ako je input za stit dat
            TryActivateShield();
        }
    }

    void TryActivateShield()
    {
        var energyCost = this.energyDepletionRate * Time.deltaTime;
        if (pilotController.shieldSignal && !shieldActive && this.CurrentEnergy >= energyCost)
        {
            TurnOnProtection();
        }
        else 
        {
            DeactivateShield();
        }
    }

    void TurnOnProtection()
    {
        //do protection stuff
        shieldActive = true;
        //Debug.Log("Ship is protected");
    }

    void KeepShieldUp()
    {
        var energyCost = this.energyDepletionRate * Time.deltaTime;
        this.TryChangeCurrentEnergy(-energyCost);
        if (CurrentEnergy == 0f || !pilotController.shieldSignal) DeactivateShield();
        else shieldUpTime += Time.deltaTime;

        if (shieldActive && shieldUpTime >= minInvulnerabilityTime)
        {
            IsInvulnerable = true;
            if (!shieldVisuals.activeInHierarchy) shieldVisuals.SetActive(true);
        }
    }

    void DeactivateShield()
    {
        shieldActive = false;
        IsInvulnerable = false;
        shieldUpTime = 0f;
        shieldVisuals.SetActive(false);
    }

}
