
using UnityEngine;

public class ShipSystems : MonoBehaviour {
    // assign in inspector
    public PilotController pilotController;

    ShipSystem[] allSystems;

    private void Awake() {
        FindSystemsAndInitializeThem();
    }

    void FindSystemsAndInitializeThem() {
        allSystems = GetComponentsInChildren<ShipSystem>();
        foreach (var system in allSystems) system.Initialize(this);
    }

    private void FixedUpdate() {
        foreach (var system in allSystems) system.UpdateLogic();
    }
}