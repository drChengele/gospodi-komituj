using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanisterGenerator : MonoBehaviour, IObjectGenerator {
    [SerializeField] Transform solipsist;
    [SerializeField] float spawnTimeMin;
    [SerializeField] float spawnTimeMax;
    [SerializeField] float distanceOfSpawn;
    [SerializeField] float transversalOffset;
    [SerializeField] GameObject createWhat;

    float timeLeftTilSpawnCycle = 0f;

    private void Update() {
        timeLeftTilSpawnCycle -= Time.deltaTime;
        if (timeLeftTilSpawnCycle < 0f) {
            timeLeftTilSpawnCycle = UnityEngine.Random.Range(spawnTimeMin, spawnTimeMax);
            DoDespawnPass();
            GenerateCanisterPair();
        }
    }

    public event Action<GameObject> ObjectGenerated;

    HashSet<GameObject> maintainedObjects = new HashSet<GameObject>();

    void GenerateCanisterPair() {
        var yOffset = UnityEngine.Random.Range(-1f, 1f) * transversalOffset;
        var xOffset = UnityEngine.Random.Range(-1f, 1f) * transversalOffset;
        var worldCoords = solipsist.transform.position + new Vector3(xOffset, yOffset, distanceOfSpawn);

        //var offset = solipsist.transform.localRotation * Vector3.right * 20f; // spawn radius

        //for (var i = 0; i < 2; i++) {
            var firstCanisterInstance = Instantiate(createWhat);
            firstCanisterInstance.transform.position = worldCoords;
            maintainedObjects.Add(firstCanisterInstance);
            ObjectGenerated?.Invoke(firstCanisterInstance);
        //}
    }

    private void DoDespawnPass() {
        var toRemove = maintainedObjects.Where(IsObjectRipeForDestruction).ToArray();
        maintainedObjects.ExceptWith(toRemove);
        foreach (var obj in toRemove)
            Destroy(obj);
    }

    bool IsObjectRipeForDestruction(GameObject obj) => solipsist.transform.InverseTransformPoint(obj.transform.position).z < 0f;

}
