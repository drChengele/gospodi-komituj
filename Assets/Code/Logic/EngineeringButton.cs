using System;
using UnityEngine;

public class EngineeringButton : MonoBehaviour, IEngineerInteractible {

    [SerializeField] WutDuzDisBUttonDo buttonFunction;

    public void OnHoldContinued() {
        
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
                break;
            case WutDuzDisBUttonDo.ChargeWeapon:
                break;
            case WutDuzDisBUttonDo.ChargeEngine:
                break;
            default:
                break;
        }
    }

    public enum WutDuzDisBUttonDo {
        ChargeShields,
        ChargeWeapon,
        ChargeEngine,
        // add here
    }
}