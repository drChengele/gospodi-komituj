using System;
using System.Collections.Generic;
using UnityEngine;

public class EngineeringCameraEffectsManager : MonoBehaviour {

    [SerializeField] Vector3 inertiaMultiplier;
    [SerializeField] Transform cameraPivot;
    [SerializeField] float cameraPromptness;

    IInertiaProvider InertiaSource => ObjectManager.Instance.InertiaSource;

    void Update() {
        ApplyInertia(InertiaSource.CurrentAccelerationRelative);
    }

    Vector3 _vel;

    void ApplyInertia(Vector3 inertiaForce) {
        var desiredCameraOffset = Vector3.Scale(inertiaForce, inertiaMultiplier);
        cameraPivot.localPosition = Vector3.SmoothDamp(cameraPivot.localPosition, desiredCameraOffset, ref _vel, cameraPromptness);
    }
}