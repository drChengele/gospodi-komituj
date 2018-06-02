using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with engineer mouse UI (interacting with engineering stuff : buttons, sliders, tools and debris
/// Engineer can CLICK buttons
/// Engineer can GRAB, DRAG and THROW debris and tools
/// </summary>
public class EngineeringController : MonoBehaviour {

    [SerializeField] Transform junkLayerRoot; // this is where tools and debris float
    [SerializeField] [Range(0f, 2f)] float retainShipInertia;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MouseHoldStarted();
        } else if (Input.GetMouseButtonUp(0)) {
            MouseReleased();
        } else if (Input.GetMouseButton(0)) {
            MouseHeld();
        }
    }

    private void FixedUpdate() {
        ProcessJunkPhysics();
    }

    private void ProcessJunkPhysics() {
        var allObjects = junkLayerRoot.GetComponentsInChildren<Grabbable>();
        var acceleration = ObjectManager.Instance.InertiaSource.CurrentAccelerationRelative * -retainShipInertia;

        foreach (var grabbable in allObjects) grabbable.Rigidbody.AddForce(acceleration * grabbable.receivedInertia, ForceMode.Acceleration);
    }

    private void MouseHeld() {
        
    }

    private void MouseReleased() {
        
    }

    private void MouseHoldStarted() {
        
    }


}