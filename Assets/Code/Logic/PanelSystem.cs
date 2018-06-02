using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSystem : ShipSystem , IEngineerInteractible {

    [SerializeField] float rechargeRate;
    bool isCharging = false;
    public bool isPanel = true;

    private enum DamageState
    {
        Operational,
        Malfunction,
        Destroyed,
    }


    [SerializeField] DamageState currentDamageState;

    private void Awake()
    {
        this.DefaultEnergy(50f);
        //if (currentDamageState == null) currentDamageState = DamageState.Operational;
    }

    public void ChargePanel(bool charge)
    {
        if (charge) this.TryChangeCurrentEnergy(rechargeRate * Time.deltaTime);
    }

    void DepletePanel()
    {
        if (this.CurrentEnergy > 0f)
        {
            this.TryChangeCurrentEnergy(-energyDepletionRate * Time.deltaTime);
        }
    }

    public void ActivateCharging()
    {
        if (currentDamageState == DamageState.Malfunction && !isCharging) isCharging = true;
    }

    public void DeactivateCharging()
    {
        isCharging = false;
    }

    private void Update()
    {
        switch (currentDamageState)
        {
            case DamageState.Operational:
                break;
            case DamageState.Malfunction:
                DepletePanel();
                ChargePanel(isCharging);
                DeactivateCharging();
                Debug.Log(this.CurrentEnergy);
                break;
            case DamageState.Destroyed:
                break;
            default:
                break;
        }
    }

    public void OnHoldStarted()
    {
        //throw new System.NotImplementedException();
    }

    public void OnHoldContinued()
    {
        //throw new System.NotImplementedException();
    }

    public void OnHoldReleased()
    {
       // throw new System.NotImplementedException();
    }
}
