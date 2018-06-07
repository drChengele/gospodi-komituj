using System;
using System.Collections.Generic;
using UnityEngine;


public class FudgedInfiniteObjectField : MonoBehaviour, IObjectGenerator {
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 cubeDimensions;
    [SerializeField] float relocationLeeway;
    [SerializeField] float objectDensity;
    [SerializeField] int updateStep;

    Vector3 extents;
    Vector3 expandedExtents; // extents + relocation leeways

    List<FudgedInfiniteObject> maintainedObjects = new List<FudgedInfiniteObject>();

    private void Start() {
        extents = cubeDimensions * 0.5f;
        expandedExtents = extents + Vector3.one * relocationLeeway;

        GenerateAll();
    }

    public event Action<GameObject> ObjectGenerated;
    
    public void GenerateAll() {
        int numObjects = Mathf.FloorToInt(cubeDimensions.x * cubeDimensions.y * cubeDimensions.z * objectDensity / 1000000f);
        for (var i = 0; i < numObjects; i++) {
            // spawn object 
            var coords = Vector3.Scale(Utility.GetRandomVector(), extents);
            coords = transform.TransformPoint(coords);
            var @object = Instantiate(prefab, coords, Utility.GetRandomRotation());
            var fo = @object.GetComponent<FudgedInfiniteObject>();
            fo.Initialize(this);
            maintainedObjects.Add(fo);
            ObjectGenerated?.Invoke(@object);
        }
        Debug.Log($"Infinite object field spawned {maintainedObjects.Count} objects");
    }

    long framecounter = 0;
    void Update() {
        framecounter++;
        var offset = (int)(framecounter % updateStep);
        for (var i = offset; i < maintainedObjects.Count; i+= updateStep)
            UpdateObjectPosition(maintainedObjects[i]);
    }

    public enum Zone {
        CentralZone,
        PositiveX,
        NegativeX, 
        PositiveY, 
        NegativeY,
        PositiveZ,
        NegativeZ
    }

    void UpdateObjectPosition(FudgedInfiniteObject target) {
        var localCoords = transform.InverseTransformPoint(target.transform.position);
        var zone = GetZone(localCoords);
        if (zone == Zone.CentralZone) return;
        var relocationTargetZone = GetInverseZone(zone);
        var targetCoords = GetRandomLocalPositionInZone(relocationTargetZone);
        var worldTargetCoords = transform.TransformPoint(targetCoords);
        target.transform.position = worldTargetCoords;
        target.transform.rotation = Utility.GetRandomRotation();
        target.Respawn();
    }

    Vector3 GetRandomLocalPositionInZone(Zone zone) {
        var v = Vector3.Scale(Utility.GetRandomVector(), extents);
        float randomAdd = UnityEngine.Random.Range(0f, relocationLeeway);

        switch(zone) {
            case Zone.PositiveX: v.x =  extents.x + randomAdd; break;
            case Zone.NegativeX: v.x = -extents.x - randomAdd; break;
            case Zone.PositiveY: v.y = extents.y + randomAdd; break;
            case Zone.NegativeY: v.y = -extents.y - randomAdd; break;
            case Zone.PositiveZ: v.z = extents.z + randomAdd; break;
            case Zone.NegativeZ: v.z = -extents.z - randomAdd; break;
            default: throw new InvalidOperationException("Invalid zone");
        }
        return v;
    }
    
    Zone GetInverseZone(Zone zone) {
        switch (zone) {
            case Zone.PositiveX: return Zone.NegativeX;
            case Zone.NegativeX: return Zone.PositiveX;
            case Zone.PositiveY: return Zone.NegativeY;                
            case Zone.NegativeY: return Zone.PositiveY;
            case Zone.PositiveZ: return Zone.NegativeZ;
            case Zone.NegativeZ: return Zone.PositiveZ;
        }
        return Zone.CentralZone;
    }

    Zone GetZone(Vector3 localCoords) {
        if (localCoords.x > expandedExtents.x) return Zone.PositiveX;
        if (localCoords.y > expandedExtents.y) return Zone.PositiveY;
        if (localCoords.z > expandedExtents.z) return Zone.PositiveZ;
        if (localCoords.x < -expandedExtents.x) return Zone.NegativeX;
        if (localCoords.y < -expandedExtents.y) return Zone.NegativeY;
        if (localCoords.z < -expandedExtents.z) return Zone.NegativeZ;
        return Zone.CentralZone;
    }
}

static public partial class Utility {
    static public Vector3 GetRandomVector() {
        return new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        );
    }
}