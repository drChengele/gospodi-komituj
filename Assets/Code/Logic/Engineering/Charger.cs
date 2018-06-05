
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class Charger : MonoBehaviour {
    private void Awake() {
        SubscribeToEvents();
    }

    private void SubscribeToEvents() {
        var grabbable = GetComponent<Grabbable>();
        grabbable.HoldResumed += OnHoldResumed; ;
    }

    private void OnHoldResumed(Grabbable grabbable) {
        var panelToCharge = grabbable.GetComponent<Interactor>()?.GetInteractorTargetUnderneathMe() as PanelSystem;
        panelToCharge?.ActivateCharging();
    }
}
