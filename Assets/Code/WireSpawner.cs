using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSpawner : MonoBehaviour {

    [SerializeField] Material[] materials;
    [SerializeField] Transform wireParent;
    [SerializeField] GameObject wire;
    [SerializeField] Animator klapna;

    [SerializeField] float wireSpawnInterval;

    [SerializeField] public GameObject wirePrefabInSituFull;
    [SerializeField] public GameObject wirePrefabInSituMalfunction;
    [SerializeField] public GameObject wirePrefabInSituDestroyed;

    public Material GetMaterialFor(WireType type) {
        return materials[(int)type];
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            EnqueueWireSpawn(Utility.GetRandomWire());
        }

        ProcessQueue();
    }

    IEnumerator WireSpawnCoroutine(WireType type) {
        var go = (GameObject)Instantiate(wire, transform.position, Quaternion.identity, wireParent);
        go.GetComponent<WireBehaviour>().SetWireType(type);
        klapna.SetTrigger("Open");
        yield return new WaitForSeconds(0.6f);
        go.GetComponent<WireSpawnAnimator>().StartAnimation();
    }

    float timeToNextWire;

    void ProcessQueue() {
        timeToNextWire -= Time.deltaTime; if (timeToNextWire > 0f) return;
        if (wiresToSpawn.Count == 0) return;

        StartCoroutine(WireSpawnCoroutine(wiresToSpawn.Dequeue()));

        timeToNextWire = wireSpawnInterval;
    }

    Queue<WireType> wiresToSpawn = new Queue<WireType>();

    public void EnqueueWireSpawn(WireType wire) {
        wiresToSpawn.Enqueue(wire);
    }
}
