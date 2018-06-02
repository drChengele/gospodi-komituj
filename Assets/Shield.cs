using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ShipSystem {

    bool shieldActive = false;

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
        if (!shieldActive && this.CurrentEnergy >= energyCost)
        {
            this.TryChangeCurrentEnergy(energyCost);
            TurnOnProtection();
        }
        else
        {
            //inform pilot to inform engi to gib powa to shield
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
        if (this.CurrentEnergy >= energyCost)
        {
            this.TryChangeCurrentEnergy(energyCost);
        }
        else
        {
            DeactivateShield();
        }
    }

    void DeactivateShield()
    {
        //turn off shield
        shieldActive = false;
        //Debug.Log("Ship is no longer protected");
    }

}
