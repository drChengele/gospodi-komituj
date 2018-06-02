using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipReactor : ShipSystem {

    [SerializeField] float coolingRate;

    protected override void TooMuchEnergy()
    {
        
    }

    private void FixedUpdate()
    {
        this.TryChangeCurrentEnergy(-coolingRate * Time.fixedDeltaTime);
    }

    protected override void OutOfEnergy()
    {
       
    }
}
