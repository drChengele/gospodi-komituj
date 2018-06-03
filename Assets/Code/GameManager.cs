using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public void ProcessAsteroidShipCollision(GameObject asteroid ) {
        ObjectManager.Instance.CockpitEffects.AddCockpitShake(6f);
        // todo: shake up electronics
        DamageRandomShipSystem();
    }

    public void ProcessBulletAsteroidCollision(GameObject bullet, GameObject asteroid) {

    }

    public void DamageRandomShipSystem() {

        // get random panel
        var allPanels = FindObjectsOfType<PanelSystem>();

        var functionalPanels = allPanels.Where(panel => panel.CurrentDamageState == DamageState.Operational).ToArray();

        if (functionalPanels.Length == 0)
            GameOver(false);
        else 
            functionalPanels[UnityEngine.Random.Range(0, functionalPanels.Length)].ChangeDamageState(DamageState.Malfunction);
    }

    private void GameOver(bool success) {
        
    }

    internal void AsteroidWasHitByBullet(Bullet bullet, GameObject asteroid) {
        // destroy asteroid
        // instantiate explosion
        // instantiate chunks
    }

    internal void CanisterWasHitByBullet(Bullet bullet, GameObject canister) {
        // instantiate explosion
    }

    internal void ShipPickedUpCanister(Canister canister) {
        ObjectManager.Instance.WireSpawner.SpawnWire(canister.wireType);
        canister.gameObject.SetActive(false); // maintainer will destroy it later, just hide it for now
    }

    public void AttemptedWireSlotting(WireBehaviour wire, PanelSystem panel) {
        if (panel.CurrentDamageState == DamageState.Malfunction) {
            if (panel.CanSlotWire(wire)) {
                panel.SlotWire(wire);
            }
        } 
    }

    internal void PanelBroken(PanelSystem panelSystem) {
        throw new NotImplementedException();
    }

    internal void PanelDestroyed(PanelSystem panelSystem) {
        throw new NotImplementedException();
    }

    internal void PanelMadeOperational(PanelSystem panelSystem) {
        throw new NotImplementedException();
    }
}

