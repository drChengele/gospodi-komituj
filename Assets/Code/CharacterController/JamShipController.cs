using System;
using UnityEngine;

public class JamShipController : MonoBehaviour, IEngineEffector {
    public ShipSystems systems;

    [Range(0f, 10f)]
    public float thrustVertical;

    [Range(0f, 10f)]
    public float thrustTransversal;

    public float forwardSpeed;

    [Range(0.92f, 0.999f)]
    public float inertiaRetain;

    public Vector3 CurrentVelocity => currentVelocity;

    Vector3 currentVelocity;
    
    void Update () {
        ApplyVelocity();
	}

    private void FixedUpdate() {
        ApplyIntertiaAndFriction();
    }

    private void ApplyIntertiaAndFriction() {
        currentVelocity.x *= inertiaRetain;
        currentVelocity.y *= inertiaRetain;
    }

    public void ApplyThrust(Vector2 thrust) {
        thrust.Scale(new Vector2(thrustTransversal, thrustVertical));
        Vector3 finalThrustAddPerSecond = thrust;
        currentVelocity += finalThrustAddPerSecond * Time.fixedDeltaTime;
    }

    void ApplyVelocity() {
        FixVelocity();
        transform.position += CurrentVelocity * Time.deltaTime;
    }

    void FixVelocity() {
        currentVelocity.z = forwardSpeed;
    }
}
