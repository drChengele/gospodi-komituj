using System;
using UnityEngine;

public class EngineeringButton : MonoBehaviour, IEngineerInteractible {

    [SerializeField] WutDuzDisBUttonDo buttonFunction;

    [SerializeField] float rechargeEnergyRate;
    [SerializeField] float rechargeHeatRate;

    float initialZ;
    float targetZ;

    void Awake() {
        var rememberedZ = transform.localPosition.z;
    }

    void LateUpdate() {
        var pos = transform.localPosition;
        pos.z = targetZ;
        transform.localPosition = pos;
        targetZ = initialZ;
        // update depressio:
    }


    public void OnHoldContinued() {
        targetZ = -0.0977f;
        switch (buttonFunction)
        {

            //ObjectManager.Instance

            case WutDuzDisBUttonDo.ChargeShields:
                ObjectManager.Instance.ShipSystems.Shield.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeWeapon:
                ObjectManager.Instance.ShipSystems.Weapon.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeEngine:
                ObjectManager.Instance.ShipSystems.Engine.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.CoolReactor:
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                break;
            default:
                break;
        }
    }
    public void OnHoldReleased() {
        
    }

    public void OnHoldStarted() {
        ButtonWasClicked();
    }

    void ButtonWasClicked() {
        switch (buttonFunction) {

            //ObjectManager.Instance

            case WutDuzDisBUttonDo.ChargeShields:
                ObjectManager.Instance.ShipSystems.Shield.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeWeapon:
                ObjectManager.Instance.ShipSystems.Weapon.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeEngine:
                ObjectManager.Instance.ShipSystems.Engine.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.CoolReactor:
                ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                break;
            default:
                break;
        }
    }

    float ContineousRecharging(float baseAmount)
    {
        return baseAmount * Time.deltaTime;

    }

    public enum WutDuzDisBUttonDo {
        ChargeShields,
        ChargeWeapon,
        ChargeEngine,
        CoolReactor,
    }
}