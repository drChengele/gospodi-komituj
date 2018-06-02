using System;
using System.Collections.Generic;
using UnityEngine;

public class OutsideCameraEffectsManager : MonoBehaviour {
    [SerializeField] JamShipController shipController;
    [SerializeField] Transform outsideCameraPivot;
    [SerializeField] [Range(-10f, 10f)] float pitchFactor;
    [SerializeField] [Range(-10f, 10f)] float yawFactor;
    [SerializeField] [Range(-20f, 20f)] float rollFactor;
    [SerializeField] [Range(0f, 1f)] float cameraResponsiveness;

    private void Update() {
        ApplyCameraPivot();
    }

    private void ApplyCameraPivot() {
        var pitch = shipController.CurrentVelocity.y * pitchFactor;
        var yaw   = shipController.CurrentVelocity.x * yawFactor;
        var roll  = shipController.CurrentVelocity.x * rollFactor;

        var targetRotation = Quaternion.Euler(pitch, yaw, roll);
        outsideCameraPivot.transform.localRotation = Quaternion.Slerp(outsideCameraPivot.transform.localRotation, targetRotation, cameraResponsiveness);
    }
}