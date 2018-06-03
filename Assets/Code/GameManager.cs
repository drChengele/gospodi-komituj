using System;
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

    }

    internal void AsteroidWasHitByBullet(Bullet bullet, GameObject asteroid) {
        // destroy asteroid
        // instantiate explosion
        // instantiate chunks
    }

    internal void CanisterWasHitByBullet(Bullet bullet, GameObject canister) {
        // destroy canister
        // instantiate explosion
    }

    internal void ShipPickedUpCanister(Canister canister) {
        
    }
}

