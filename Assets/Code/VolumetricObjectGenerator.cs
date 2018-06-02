using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricObjectGenerator : MonoBehaviour {

    public enum SpawnObjectType
    {
        Asteroid,
        Debris,
        Resources,
        RandomStuff
    }

    public SpawnerData[] spawnerData;

    public Vector3 volumeSize;

    private Vector3 minCorner;

    private SpawnParameters spawnParameters;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < spawnerData.Length; i++)
        {
            if (spawnerData[i].canSpawn)
            {
                if (spawnerData[i].currentTime >= spawnerData[i].cooldown)
                {
                    spawnerData[i].currentTime = 0f;
                    this.GenerateObject(spawnerData[i]);
                }
                else spawnerData[i].currentTime += Time.deltaTime;
            }

        }
    }

    void GenerateObject(SpawnerData data)
    {
        
        var objectToSpawn = data.ObjectToSpawn();
        var spnParams = CalculateSpawnParameters(data);
        
        for (int i = 0; i < 100; i++)
        {
            if (CheckSpawnability(objectToSpawn, spnParams.position, spnParams.rotation, spnParams.scale))
            {
                Instantiate(objectToSpawn, spnParams.position, spnParams.rotation);
                objectToSpawn.transform.localScale = spnParams.scale;
                break;
            }
            else
            {
                objectToSpawn = data.ObjectToSpawn();
                spnParams = CalculateSpawnParameters(data);
            }
        }
        
        
    }

    SpawnParameters CalculateSpawnParameters(SpawnerData data)
    {
        var parameters = new SpawnParameters();
        parameters.position = new Vector3(transform.position.x + Random.Range(-volumeSize.x / 2, volumeSize.x / 2),
                                    transform.position.y + Random.Range(-volumeSize.y / 2, volumeSize.y / 2),
                                    transform.position.z + Random.Range(-volumeSize.z / 2, volumeSize.z / 2));
        parameters.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        parameters.scale = new Vector3(data.CalculateScale(), data.CalculateScale(), data.CalculateScale());

        return parameters;
    }

    bool CheckSpawnability(GameObject obj, Vector3 pos, Quaternion rot, Vector3 scl) 
    {

        var isValid = false;
        var bounds = obj.GetComponent<Collider>().bounds;
        Collider[] hitStuff = Physics.OverlapSphere(pos, Vector3.Distance(bounds.max,bounds.min) * Mathf.Max(scl.x,scl.y,scl.z)/2);
        if (hitStuff.Length == 0) isValid = true;
        return isValid; 
    }

    [System.Serializable]
    public class SpawnerData {
        public string name;
        
        public SpawnObjectType type;
        public bool canSpawn = true;
        [HideInInspector]
        public float currentTime = 0f;
        public float cooldown = 0.5f;
        public GameObject[] objectsToSpawn;
        [Range(0.01f, 10f)]
        public float minScale;
        [Range(0.5f, 100f)]
        public float maxScale;

        public GameObject ObjectToSpawn()
        {
            return objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        }

        public float CalculateScale()
        {
            return Random.Range(minScale, maxScale);
        }
    }

    public struct SpawnParameters
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public SpawnParameters(Vector3 pos, Quaternion rot, Vector3 scl)
        {
            position = pos;
            rotation = rot;
            scale = scl;
        }
    }
}
