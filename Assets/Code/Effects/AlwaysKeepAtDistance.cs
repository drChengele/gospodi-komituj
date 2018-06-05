using System.Collections.Generic;
using UnityEngine;


public class AlwaysKeepAtDistance : MonoBehaviour {
    [SerializeField] Transform from;
    [SerializeField] Vector3 offset;

    void Update() {
        transform.position = from.transform.position + offset;
    }
}