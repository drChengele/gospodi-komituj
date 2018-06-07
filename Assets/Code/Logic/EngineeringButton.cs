using System;
using UnityEngine;

public class EngineeringButton : MonoBehaviour, IEngineerInteractible {

    [SerializeField] WutDuzDisBUttonDo buttonFunction;

    [SerializeField] float rechargeEnergyRate;

    [SerializeField] Transform visualMesh;
    //[SerializeField] float rechargeHeatRate;

    [SerializeField] Vector3 buttonPressDelta;

    bool isDepressed;
    Color inactiveColor;
    Color activatedColor;

    void Awake() {
        activatedColor = visualMesh.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        inactiveColor = activatedColor;
        inactiveColor.r *= 0.3f; inactiveColor.g *= 0.3f; inactiveColor.b *= 0.3f;        
    }

    void LateUpdate() {
        var target = isDepressed ? buttonPressDelta : Vector3.zero;
        visualMesh.localPosition = Vector3.MoveTowards(visualMesh.localPosition, target, 0.6f * Time.deltaTime);
        SetEmissionIntensity(visualMesh.GetComponent<MeshRenderer>(), isDepressed ? 3 : 0.1f);
        isDepressed = false; // reset, onholdcontinued will set it to true if necessary
    }

    void SetEmissionIntensity(MeshRenderer renderer, float newIntensity) {
        renderer.material.SetColor("_EmissionColor", isDepressed ? activatedColor : inactiveColor);
    }

    public void OnHoldContinued() {
        isDepressed = true;
        switch (buttonFunction)
        {
            //ObjectManager.Instance

            case WutDuzDisBUttonDo.ChargeShields:
                ObjectManager.Instance.ShipSystems.Shield.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeWeapon:
                ObjectManager.Instance.ShipSystems.Weapon.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeEngine:
                ObjectManager.Instance.ShipSystems.Engine.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.CoolReactor:
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
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
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeWeapon:
                ObjectManager.Instance.ShipSystems.Weapon.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
                break;
            case WutDuzDisBUttonDo.ChargeEngine:
                ObjectManager.Instance.ShipSystems.Engine.TryChangeCurrentEnergy(ContineousRecharging(rechargeEnergyRate));
                //ObjectManager.Instance.ShipSystems.Reactor.TryChangeCurrentEnergy(ContineousRecharging(rechargeHeatRate));
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