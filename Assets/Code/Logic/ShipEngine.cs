using System;
using UnityEngine;

public class ShipEngine : ShipSystem {
    Vector2 thrustSignal;
    float rollSignal;
    float forwardSignal;

    public IEngineEffector EngineEffector => ObjectManager.Instance.ShipController as IEngineEffector;

    internal override void UpdateFrameLogic() {
        base.UpdateFrameLogic();
        // just read the state of the thruster signal
        thrustSignal = ObjectManager.Instance.PilotController.thrusterSignal;
        rollSignal = ObjectManager.Instance.PilotController.rollSignal;
        forwardSignal = ObjectManager.Instance.PilotController.forwardSignal;
    }

    internal override void UpdateFixedLogic() {
        base.UpdateFixedLogic();
        ApplySignalToLateralThrusters();
    }

    void ApplySignalToLateralThrusters() {
        float expenditure = thrustSignal.magnitude + Mathf.Abs(rollSignal) * 0.5f;
        if (expenditure > 1f) expenditure = 1f;
        TryChangeCurrentEnergy(-expenditure * energyDepletionRate * Time.deltaTime);
        if (CurrentEnergy > float.Epsilon) {
            EngineEffector.ApplyLateralThrust(thrustSignal);
            EngineEffector.ApplyRoll(rollSignal);
            EngineEffector.ApplyForwardThrust(forwardSignal);
        }
    }
}

public interface IEngineEffector {
    void ApplyRoll(float roll);

    /// <summary>Expects thrust vector NOT GREATER THAN ONE</summary>
    void ApplyLateralThrust(Vector2 thrust);
    void ApplyForwardThrust(float forwardSignal);
}