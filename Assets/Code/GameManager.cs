﻿using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public void ProcessAsteroidShipCollision(GameObject asteroid ) {
        if (ObjectManager.Instance.ShipSystems.Shield.IsInvulnerable) {
            asteroid.SetActive(false);
        } else {            
            DamageRandomShipSystem();
            // todo: shake up electronics
        }
        ObjectManager.Instance.CockpitEffects.AddCockpitShake(6f);
    }

    public void ProcessBulletAsteroidCollision(GameObject bullet, GameObject asteroid) {

    }

    public void DamageRandomShipSystem() {

        // get random panel
        var allPanels = FindObjectsOfType<PanelSystem>();

        var functionalPanels = allPanels.Where(panel => panel.CurrentDamageState == DamageState.Operational).ToArray();

        if (functionalPanels.Length == 0) {
            
        } else
            functionalPanels[UnityEngine.Random.Range(0, functionalPanels.Length)].ChangeDamageState(DamageState.Malfunction);
    }

    static public class GlobalData {
        static public bool IsGameOverAVictory { get; set; }
        static public float Bounty { get; set; }
        static public float MetersCrossed { get; set; }
    }

    
    public void GameOver(bool success) {
        GlobalData.IsGameOverAVictory = success;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    internal void AsteroidWasHitByBullet(Bullet bullet, GameObject asteroid) {
        Instantiate(ObjectManager.Instance.Prefabs.particles_asteroidExplosion, bullet.transform.position, Quaternion.identity);
        asteroid.SetActive(false);
        bullet.DieSoon();
    }

    internal void CanisterWasHitByBullet(Bullet bullet, GameObject canister) {
        canister.SetActive(false);
        bullet.DieSoon();
    }

    internal void ShipPickedUpCanister(Canister canister) {
        ObjectManager.Instance.WireSpawner.EnqueueWireSpawn(canister.wireType);
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
        panelSystem.DefaultEnergy(50);
    }

    internal void PanelDestroyed(PanelSystem panelSystem) {
        Instantiate(ObjectManager.Instance.Prefabs.particles_smoke, panelSystem.transform.position, Quaternion.identity);
        var allPanels = FindObjectsOfType<PanelSystem>();
        var nonDestroyedPanels = allPanels.Count(panel => panel.CurrentDamageState != DamageState.Destroyed);
        if (nonDestroyedPanels == 0)
            GameOver(false);
    }

    internal void PanelMadeOperational(PanelSystem panelSystem) {
        
    }

    // make something go wrong
    public void JustFuckMyDayUpFam() {

    }

}

