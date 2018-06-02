using System;
using UnityEngine;

public class ShipEngine : ShipSystem {
    Vector2 currentSignal;

    public IEngineEffector EngineEffector => ObjectManager.Instance.ShipController;

    internal override void UpdateFrameLogic() {
        base.UpdateFrameLogic();
        // just read the state of the thruster signal
        currentSignal = ObjectManager.Instance.PilotController.thrusterSignal;
    }

    internal override void UpdateFixedLogic() {
        base.UpdateFixedLogic();
        ApplySignalToThrusters();
    }

    void ApplySignalToThrusters() {
        EngineEffector.ApplyThrust(currentSignal);
    }
}

public interface IEngineEffector {
    /// <summary>Expects thrust vector NOT GREATER THAN ONE</summary>
    void ApplyThrust(Vector2 thrust);
}