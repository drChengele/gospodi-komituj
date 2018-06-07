using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IObjectGenerator {
    event Action<GameObject> ObjectGenerated;
}

public class ObjectGenerator : MonoBehaviour, IObjectGenerator {

    private HashSet<GameObject> maintainedObjects = new HashSet<GameObject>();

    public event Action<GameObject> ObjectGenerated;

    // by definition, generation is on the Z axis
     
    [SerializeField] Transform solipsist;
    [SerializeField] float generateAhead;
    [SerializeField] float volumeHalfHeight;
    [SerializeField] float volumeHalfWidth;
    [SerializeField] float sliceDepth;


    [SerializeField] GameObject[] createWhat;
    [SerializeField] float scaleVariance;
    [SerializeField] float density; // per 100 unit of depth

    [SerializeField] int maxSpawnsPerFrame;

    [SerializeField] int minSlicesForCanister;
    [SerializeField] int maxSlicesForCanister;

    [SerializeField] GameObject canisterPrefab;
    [SerializeField] float canisterSpawnRadius;

    float maxGeneratedDepth; // last depth where stuff was generated

    float ShipProgress => solipsist.transform.position.z;

    int slicesTilCanister = 0;

    private void Start() {
        FirstGeneration();
    }

    private void FirstGeneration() {
        PopulateVolume(ShipProgress, ShipProgress + generateAhead);
    }

    private void Update() {
        if (ShouldGenerateSlice()) {
            var randomZ = maxGeneratedDepth + UnityEngine.Random.Range(0f, sliceDepth);
            PopulateVolume(maxGeneratedDepth, maxGeneratedDepth + sliceDepth);
            DoDespawnPass();
            AdvanceCanisterSpawner(randomZ);
        }
        ProcessSpawnQueue();
    }

    private void AdvanceCanisterSpawner(float z) {
        slicesTilCanister--;
        if (slicesTilCanister <= 0) {
            slicesTilCanister = UnityEngine.Random.Range(minSlicesForCanister, maxSlicesForCanister+1);
            SpawnCanisters(z);
        }
    }

    private void SpawnCanisters(float atZ) {

        Debug.Log($"Spawning canisters at z {atZ}");
        var yOffset = UnityEngine.Random.Range(-1f, 1f) * canisterSpawnRadius;
        var xOffset = UnityEngine.Random.Range(-1f, 1f) * canisterSpawnRadius;
        var worldCoords = solipsist.transform.position + new Vector3(xOffset, yOffset, atZ);

        var offset = solipsist.transform.localRotation * Vector3.right * canisterSpawnRadius;

        for (var i = 0; i < 2; i++) { 
            var firstCanisterInstance = Instantiate(canisterPrefab);
            firstCanisterInstance.transform.position = worldCoords + offset * i;
            maintainedObjects.Add(firstCanisterInstance);
            ObjectGenerated?.Invoke(firstCanisterInstance);
        }
        
     
    }

    private void DoDespawnPass() {
        var toRemove = maintainedObjects.Where(IsObjectRipeForDestruction).ToArray();
        maintainedObjects.ExceptWith(toRemove);
        foreach (var obj in toRemove)
            Destroy(obj);
    }

    bool IsObjectRipeForDestruction(GameObject obj) => solipsist.transform.InverseTransformPoint(obj.transform.position).z < 0f;

    private bool ShouldGenerateSlice() {
        return maxGeneratedDepth < ShipProgress + generateAhead - sliceDepth;
    }

    void PopulateVolume(float zStart, float zEnd) {

        float distance = (zEnd - zStart);
        var numToSpawn = (int)(distance * density * volumeHalfHeight * volumeHalfWidth / 1000000);
        for (var i = 0; i < numToSpawn; i++) {
            var item = createWhat[UnityEngine.Random.Range(0, createWhat.Length)];
            EnqueueSpawnObject(new ObjectSpawnInfo(item, UnityEngine.Random.Range(zStart, zEnd)));
        }

        Debug.Log($"Object generator objects generated: {numToSpawn}");

        maxGeneratedDepth = zEnd;
    }

    Queue<ObjectSpawnInfo> spawnQueue = new Queue<ObjectSpawnInfo>();

    class ObjectSpawnInfo {
        public ObjectSpawnInfo(GameObject prefab, float atZ) {
            this.prefab = prefab;
            this.atZ = atZ;
        }
        public readonly GameObject prefab;
        public readonly float atZ;
        public Vector3 coords;
    }

    void ProcessSpawnQueue() {
        for (var i = 0; i < maxSpawnsPerFrame; i++) {
            if (spawnQueue.Count == 0) return;
            var item = spawnQueue.Dequeue();
            item.coords = GetSpawnCoords(item);
            if (CanSpawn(item)) {
                DoSpawnObject(item);
            } else { // add it to the end of queue, try again later
                spawnQueue.Enqueue(item);
            }
        }
    }

    private bool CanSpawn(ObjectSpawnInfo item) {
        return true;
        var coord = GetSpawnCoords(item);
        var sphereCollider = item.prefab.GetComponent<SphereCollider>();
        if (sphereCollider != null) {
            var radius = sphereCollider.radius * MaxDimensionOf(item.prefab.transform.localScale);
            return Physics.OverlapSphere(coord, radius).Length == 0;
        }
        return true;
    }

    float MaxDimensionOf(Vector3 vector) => Mathf.Max(vector.x, vector.y, vector.z);

    private Vector3 GetSpawnCoords(ObjectSpawnInfo item) {
        var yOffset = UnityEngine.Random.Range(-1f, 1f) * volumeHalfHeight;
        var xOffset = UnityEngine.Random.Range(-1f, 1f) * volumeHalfWidth;
        var worldCoords = solipsist.transform.position + new Vector3(xOffset, yOffset, 0);
        worldCoords.z = item.atZ;
        return worldCoords;
    }

    private void EnqueueSpawnObject(ObjectSpawnInfo info) {
        spawnQueue.Enqueue(info);
    }

    private void DoSpawnObject(ObjectSpawnInfo info) {
        var spawnedObject = Instantiate(info.prefab) as GameObject;
        spawnedObject.transform.position = info.coords;
        spawnedObject.transform.localScale *= UnityEngine.Random.Range(1f - scaleVariance, 1f + scaleVariance);
        spawnedObject.transform.localRotation = Utility.GetRandomRotation();
        ObjectGenerated?.Invoke(spawnedObject);
        maintainedObjects.Add(spawnedObject);
    }
}

static public partial class Utility {
    public static Quaternion GetRandomRotation() {
        Func<float, float, float> rng = UnityEngine.Random.Range;
        var z = new Vector3(rng(0f, 1f), rng(0f, 1f), rng(0f, 1f));
        z *= 360f;
        return Quaternion.Euler(z);
    }

    public static Vector2 RotateVector(Vector2 vec, float angle) 
    {
        //e necu da racunam sin i cos dva puta... ima dustedim nekoliko ciklusa pa taman crko
        var sin = Mathf.Sin(Mathf.Deg2Rad * angle);
        var cos = Mathf.Cos(Mathf.Deg2Rad * angle);
        var x = vec.x * cos - vec.y * sin;
        var y = vec.x * sin + vec.y * cos;

        return new Vector2(x,y);
    }
}