using System;
using UnityEngine;

public class ShipEngine : ShipSystem {
    Vector2 thrustSignal;
    float rollSignal;

    public IEngineEffector EngineEffector => ObjectManager.Instance.ShipController as IEngineEffector;

    internal override void UpdateFrameLogic() {
        base.UpdateFrameLogic();
        // just read the state of the thruster signal
        thrustSignal = ObjectManager.Instance.PilotController.thrusterSignal;
        rollSignal = ObjectManager.Instance.PilotController.rollSignal;
    }

    internal override void UpdateFixedLogic() {
        base.UpdateFixedLogic();
        ApplySignalToThrusters();
    }

    void ApplySignalToThrusters() {
        float expenditure = thrustSignal.magnitude + Mathf.Abs(rollSignal) * 0.5f;
        if (expenditure > 1f) expenditure = 1f;
        TryChangeCurrentEnergy(-expenditure * energyDepletionRate * Time.deltaTime);
        if (CurrentEnergy > float.Epsilon) {
            EngineEffector.ApplyLateralThrust(thrustSignal);
            EngineEffector.ApplyRoll(rollSignal);
        }
    }
}

public interface IEngineEffector {
    void ApplyRoll(float roll);

    /// <summary>Expects thrust vector NOT GREATER THAN ONE</summary>
    void ApplyLateralThrust(Vector2 thrust);
}