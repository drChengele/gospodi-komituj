using System.Collections.Generic;
using UnityEngine;


public class TransformFollower : MonoBehaviour {
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 worldOffset;

    private void Update() {
        var myNextPosition = targetTransform.position + worldOffset;
        transform.position = myNextPosition;
    }
}
