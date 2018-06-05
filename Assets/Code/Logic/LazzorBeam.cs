using System;
using UnityEngine;

public class LazzorBeam : ShipSystem {

    [SerializeField] GameObject prefab;
    [SerializeField] float fireRate;
    [SerializeField] float apertureDegrees;
    [SerializeField] float tubeRandomOffset;

    internal override void UpdateFrameLogic() {
        base.UpdateFrameLogic();
        cooldownLeft -= Time.deltaTime;
        if (ObjectManager.Instance.PilotController.fireSignal) TryFire();
    }

    float cooldownLeft;

    public bool TryFire() {
        if (cooldownLeft > 0f) return false;
        TryChangeCurrentEnergy(energyDepletionRate);
        bool success = CurrentEnergy > 0.01f;
        if (success)
            Fire();
        return success;
    }

    public void Fire() {
        blasterCycle++;
        var sp = bulletSpawnPoints[blasterCycle % bulletSpawnPoints.Length];
        CreateBullet(sp);
        cooldownLeft = 1f / fireRate;
        //sp.gameObject.GetComponent<WeaponSounds>().PlayPewPew();

        ObjectManager.Instance.CockpitEffects.AddCockpitShake(0.6f);
    }

    private void CreateBullet(Transform spawnPoint) {
        var go = Instantiate(prefab) as GameObject;
        go.transform.position = spawnPoint.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * tubeRandomOffset;
        go.transform.rotation = spawnPoint.rotation;
        go.GetComponent<Bullet>().Fired();
    }

    [SerializeField] Transform[] bulletSpawnPoints;

    int blasterCycle = 0;
}