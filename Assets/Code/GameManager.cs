﻿using System;
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
        Debug.Log("ASTEROID HIT BY BULLIT");
        asteroid.SetActive(false);
        bullet.DieSoon();
        //Instantiate(ObjectManager.Instance.Prefabs.asteroidExplosion, bullet.transform.position, Quaternion.identity);
    }

    internal void CanisterWasHitByBullet(Bullet bullet, GameObject canister) {
        canister.SetActive(false);
        bullet.DieSoon();
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

    // called by panel
    internal void PanelBroken(PanelSystem panelSystem) {
        
    }

    internal void PanelDestroyed(PanelSystem panelSystem) {
        throw new NotImplementedException();
    }

    internal void PanelMadeOperational(PanelSystem panelSystem) {
        throw new NotImplementedException();
    }

    // make something go wrong
    public void JustFuckMyDayUpFam() {

    }

}

