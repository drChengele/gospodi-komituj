using System;
using UnityEngine;

public interface IInertiaProvider {
    Vector3 CurrentVelocity { get; }
    Vector3 CurrentAccelerationRelative { get; }
}

public class JamShipController : MonoBehaviour, IEngineEffector, IInertiaProvider {
    public ShipSystems systems;

    [Range(0f, 10f)]
    public float thrustVertical;

    [Range(0f, 10f)]
    public float thrustTransversal;

    public float forwardSpeed;

    [Range(0.92f, 0.999f)]
    public float inertiaRetain;

    public Vector3 CurrentVelocity => currentVelocity;
    public Vector3 CurrentAccelerationRelative => activeAcceleration;

    Vector3 currentVelocity;

    Vector3 activeAcceleration;
    
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
        activeAcceleration = thrust;
        currentVelocity += activeAcceleration * Time.fixedDeltaTime;
    }

    void ApplyVelocity() {
        FixVelocity();
        transform.position += CurrentVelocity * Time.deltaTime;
    }

    void FixVelocity() {
        currentVelocity.z = forwardSpeed;
    }
}
