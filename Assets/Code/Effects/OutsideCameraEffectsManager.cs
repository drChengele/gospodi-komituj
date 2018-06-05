using System;
using System.Collections.Generic;
using UnityEngine;

public class OutsideCameraEffectsManager : MonoBehaviour {
    [SerializeField] Transform outsideCameraPivot;
    [SerializeField] [Range(-10f, 10f)] float pitchFactor;
    [SerializeField] [Range(-10f, 10f)] float yawFactor;
    [SerializeField] [Range(-20f, 20f)] float rollFactor;
    [SerializeField] [Range(0f, 50f)] float cameraResponsiveness;

    public IInertiaProvider InertiaSource => ObjectManager.Instance.InertiaSource;

    private void Update() {
        ApplyInertia();
    }

    private void ApplyInertia() {
        var vel = InertiaSource.CurrentAccelerationRelative;

        var pitch = vel.y * pitchFactor;
        var yaw   = vel.x * yawFactor;
        var roll  = vel.x * rollFactor;

        var targetRotation = Quaternion.Euler(pitch, yaw, roll);
        outsideCameraPivot.transform.localRotation = Quaternion.Slerp(outsideCameraPivot.transform.localRotation, targetRotation, cameraResponsiveness * Time.deltaTime);
    }
}