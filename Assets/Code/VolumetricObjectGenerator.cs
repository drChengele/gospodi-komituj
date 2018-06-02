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

        var spawnPosition = new Vector3(transform.position.x + Random.Range(-volumeSize.x / 2, volumeSize.x / 2),
                                    transform.position.y + Random.Range(-volumeSize.y / 2, volumeSize.y / 2),
                                    transform.position.z + Random.Range(-volumeSize.z / 2, volumeSize.z / 2));
        Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));
        objectToSpawn.transform.localScale = new Vector3(data.CalculateScale(), data.CalculateScale(), data.CalculateScale());
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
}
