using System;
using UnityEngine;

public abstract class ShipSystem : MonoBehaviour {

    public event Action<ShipSystem> SystemHasTooMuchEnergy;
    public event Action<ShipSystem> SystemIsOutOfEnergy;

    public PilotController controller => ObjectManager.Instance.PilotController;

    public virtual void Initialize(ShipSystems myship) {
        myShip = myship;
    }

    public ShipSystems myShip { get; private set; }
    public float CurrentEnergy { get; private set; } = 50;

    public const float MinEnergy = 0f;
    public const float MaxEnergy = 100f;

    public float energyDepletionRate;

    public void TryChangeCurrentEnergy(float delta) {
        this.CurrentEnergy += delta;
        if (this.CurrentEnergy < MinEnergy && delta < 0f) { // tried to draw energy but could not
            OutOfEnergy();
            this.CurrentEnergy = MinEnergy;
        }

        if (this.CurrentEnergy > MaxEnergy && delta > 0f) { // tried to fill energy but could 
            TooMuchEnergy();
            this.CurrentEnergy = MaxEnergy;
        }
    }

    public virtual void DefaultEnergy(float energy)
    {
        this.CurrentEnergy = energy;
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