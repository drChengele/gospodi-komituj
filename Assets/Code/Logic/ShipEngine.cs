using System;
using UnityEngine;

public class ShipEngine : ShipSystem {
    Vector2 currentSignal;
    public JamShipController shipController;

    public IEngineEffector EngineEffector => shipController;

    internal override void UpdateFrameLogic() {
        base.UpdateFrameLogic();
        currentSignal = myShip.pilotController.thrusterSignal;
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