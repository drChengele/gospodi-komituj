
using System.Linq;
using UnityEngine;

public class ShipSystems : MonoBehaviour {
    ShipSystem[] allSystems;

    public ShipEngine Engine { get; private set; }
    public LazzorBeam Weapon { get; private set; }
    public ShipSystem Shield { get; private set; }

    private void Awake() {
        FindSystemsAndInitializeThem();
    }

    void FindSystemsAndInitializeThem() {
        allSystems = GetComponentsInChildren<ShipSystem>();
        foreach (var system in allSystems) system.Initialize(this);
        Engine = allSystems.OfType<ShipEngine>().First();
        Weapon = allSystems.OfType<LazzorBeam>().First();
    }

    private void FixedUpdate() {
        foreach (var system in allSystems) system.UpdateFrameLogic();
    }
}