using System;
using System.Collections.Generic;
using UnityEngine;

public class SixDofShipController : MonoBehaviour, IShipController, IEngineEffector, IInertiaProvider {
    public ShipSystems systems;

    [Range(2f, 300f)]
    public float forwardBoost;

    [Range(2f, 100f)]
    public float constantThrust;

    [Range(20f, 600f)] public float rollPower;
    [Range(20f, 600f)] public float pitchPower;
    [Range(20f, 600f)] public float yawPower;

    [Range(20f, 720f)]
    public float maxRollSpeed;

    [Range(0.92f, 0.999f)]
    public float inertiaRetain;

    public GameObject GameObject => gameObject;
    
    Vector3 currentVelocity;
    Vector3 activeAcceleration;
    Vector3 activeAccelNormalized;

    Vector3 currentRotationSpeed;

    public Vector3 CurrentVelocity => currentVelocity;
    public Vector3 CurrentAccelerationRelative => activeAcceleration;
    public Vector3 CurrentAccelerationRelativeZeroToOne => activeAccelNormalized;

    public float CurrentRollSpeed => currentRotationSpeed.z;

    void Update() {
        ApplyVelocity();
        ApplyRotation();
    }

    Vector3 targetRotationSpeed;

    private void ApplyRotation() {
        transform.Rotate(currentRotationSpeed * Time.deltaTime, Space.Self);
    }

    private void ApplyVelocity() {
        transform.position += CurrentVelocity * Time.deltaTime;
    }

    void FixedUpdate() {
        ApplyConstantThrust();
        ApplyInertiaAndFriction();
    }

    private void ApplyConstantThrust() {
        currentVelocity += transform.forward * constantThrust * Time.deltaTime;
    }

    private void ApplyInertiaAndFriction() {
        currentVelocity *= inertiaRetain;
        currentRotationSpeed *= inertiaRetain;
    }

    public void ApplyForwardThrust(float thrust) {
        currentVelocity += transform.forward * forwardBoost * thrust * Time.deltaTime;
        ObjectManager.Instance.CockpitEffects.AddCockpitShake(0.032f * thrust);
    }


    public void ApplyLateralThrust(Vector2 thrust) {
        activeAccelNormalized = thrust;
        currentRotationSpeed.x += thrust.y * pitchPower * Time.deltaTime;
        currentRotationSpeed.y += thrust.x * yawPower * Time.deltaTime;
    }

    public void ApplyRoll(float roll) {
        currentRotationSpeed.z += roll * rollPower * Time.deltaTime;
    }

}

