using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlots : MonoBehaviour {

    public float intervalMin;
    public float intervalMax;

    public ParticleSystem[] systems;

    private float intervalLeft;

    private void Awake() {
        foreach (var system in systems) {
            system.Stop();

        }
    }

    private void Update() {
        intervalLeft -= Time.deltaTime;
        if (intervalLeft < 0f) {
            intervalLeft = UnityEngine.Random.Range(intervalMin, intervalMax);
            SpawnParticles();
        }
    }

    private void SpawnParticles() {
        var system = systems[UnityEngine.Random.Range(0, systems.Length)];
        system.Play();
    }
}
