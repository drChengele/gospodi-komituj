using System;
using UnityEngine;

public class ShipEngine : ShipSystem {
    Vector2 thrustSignal;
    float rollSignal;

    public IEngineEffector EngineEffector => ObjectManager.Instance.ShipController;

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
        EngineEffector.ApplyThrust(thrustSignal);
        EngineEffector.ApplyRoll(rollSignal);
    }
}

public interface IEngineEffector {
    void ApplyRoll(float roll);

    /// <summary>Expects thrust vector NOT GREATER THAN ONE</summary>
    void ApplyThrust(Vector2 thrust);
}