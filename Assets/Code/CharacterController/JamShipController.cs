using System;
using UnityEngine;

public interface IInertiaProvider {
    Vector3 CurrentVelocity { get; }
    Vector3 CurrentAccelerationRelative { get; }
    Vector3 CurrentAccelerationRelativeZeroToOne { get; }
    float   CurrentRollSpeed { get; }
}

public interface IShipController {
    GameObject GameObject { get; }
}

public class JamShipController : MonoBehaviour, IShipController, IEngineEffector, IInertiaProvider {
    public ShipSystems systems;

    [Range(2f, 60f)]
    public float thrustPower;

    [Range(20f, 600f)]
    public float rollPower;

    [Range(20f, 720f)]
    public float maxRollSpeed;

    public float forwardSpeed;

    [Range(0.92f, 0.999f)]
    public float inertiaRetain;

    public GameObject GameObject => gameObject;

    public Vector3 CurrentVelocity => currentVelocity;
    public Vector3 CurrentAccelerationRelative => activeAcceleration;
    public Vector3 CurrentAccelerationRelativeZeroToOne => activeAccelNormalized;

    public float CurrentRollSpeed => currentRoll;

    Vector3 currentVelocity;
    Vector3 activeAcceleration;
    Vector3 activeAccelNormalized;

    float currentRoll;
    
    void Update () {
        ApplyVelocity();
        UpdateRollTation();
	}

    private void FixedUpdate() {
        ApplyIntertiaAndFriction();
    }

    private void ApplyIntertiaAndFriction() {
        currentVelocity.x *= inertiaRetain;
        currentVelocity.y *= inertiaRetain;
        //currentRoll *= inertiaRetain;
    }

    public void ApplyLateralThrust(Vector2 thrust) {
        activeAccelNormalized = thrust;
        activeAcceleration = thrust * this.thrustPower;

        var transformedVector = transform.TransformVector(activeAcceleration); // take rotation into account

        currentVelocity += transformedVector * Time.fixedDeltaTime;
    }

    public void ApplyForwardThrust(float thrust) {
        // do nothing, we go forward all the time baby
    }

    void ApplyVelocity() {
        FixVelocity();
        transform.position += CurrentVelocity * Time.deltaTime;
    }

    void UpdateRollTation() {
        currentRoll = Mathf.MoveTowards(currentRoll, targetRoll, rollPower * Time.deltaTime);
        transform.Rotate(0, 0, currentRoll * Time.deltaTime, Space.Self);
    }

    float targetRoll;

    public void ApplyRoll(float roll) {
        targetRoll = roll * maxRollSpeed;
    }

    void FixVelocity() {
        currentVelocity.z = forwardSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Asteroid") {
            ObjectManager.Instance.GameManager.ProcessAsteroidShipCollision(other.gameObject);
        } else if (other.gameObject.tag == "Canister") {
            ObjectManager.Instance.GameManager.ShipPickedUpCanister(other.gameObject.GetComponent<Canister>());
        }
    }
}
