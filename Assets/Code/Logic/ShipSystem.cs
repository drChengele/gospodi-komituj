using System;
using UnityEngine;

public abstract class ShipSystem : MonoBehaviour {

    public event Action<ShipSystem> SystemHasTooMuchEnergy;
    public event Action<ShipSystem> SystemIsOutOfEnergy;

    public PilotController controller => myShip.pilotController;

    public virtual void Initialize(ShipSystems myship) {
        myShip = myship;
    }

    public ShipSystems myShip { get; private set; }
    public float CurrentEnergy { get; private set; }

    const float MinEnergy = 0f;
    const float MaxEnergy = 100f;

    public void TryChangeCurrentEnergy(float delta) {
        this.CurrentEnergy += delta;
        if (this.CurrentEnergy < MinEnergy && delta < 0f) { // tried to draw energy but could not
            OutOfEnergy();
        }

        if (this.CurrentEnergy > MaxEnergy && delta > 0f) { // tried to fill energy but could 
            TooMuchEnergy();
        }
    }

    private void FixedUpdate() {
        UpdateFixedLogic();
    }

    private void Update() {
        UpdateFrameLogic();
    }

    protected virtual void TooMuchEnergy() {
        SystemHasTooMuchEnergy?.Invoke(this);
    }

    protected virtual void OutOfEnergy() {
        SystemIsOutOfEnergy?.Invoke(this);
    }

    internal virtual void UpdateFrameLogic() { }
    internal virtual void UpdateFixedLogic() { }
}