using System.Collections.Generic;
using UnityEngine;


public class CockpitController : MonoBehaviour {
    [SerializeField] public LampGroup weaponIndicator;
    [SerializeField] public LampGroup shieldIndicator;
    [SerializeField] public LampGroup engineIndicator;

    private void Update() {
        engineIndicator.SetLevel(ObjectManager.Instance.ShipSystems.Engine.CurrentEnergy / 100f);
        shieldIndicator.SetLevel(ObjectManager.Instance.ShipSystems.Shield.CurrentEnergy / 100f);
        weaponIndicator.SetLevel(ObjectManager.Instance.ShipSystems.Weapon.CurrentEnergy / 100f);
    }
}
