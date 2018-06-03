using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSpawner : MonoBehaviour {

    [SerializeField] Material[] materials;
    [SerializeField] Transform wireParent;
    [SerializeField] GameObject wire;
    [SerializeField] Transform spawnOrigin;

    [SerializeField] public GameObject wirePrefabInSituFull;
    [SerializeField] public GameObject wirePrefabInSituMalfunction;
    [SerializeField] public GameObject wirePrefabInSituDestroyed;


    public void SpawnWire(WireType wType)
    {
        var go = (GameObject)Instantiate(wire, spawnOrigin.position, Quaternion.identity, wireParent);
        go.GetComponent<WireBehaviour>().wType = wType;
        go.GetComponent<MeshRenderer>().material = materials[(int)wType];
    }
}
