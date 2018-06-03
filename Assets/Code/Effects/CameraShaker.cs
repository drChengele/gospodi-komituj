using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
    [SerializeField] float ferocityRetain;
    [SerializeField] float exaggeration;
    float ferocity;

    public void AddFerocity(float howMuch) {
        ferocity += howMuch;
    }

    private void FixedUpdate() {
        ferocity *= ferocityRetain;
    }

    private void Update() {
        var v = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        v *= ferocity * exaggeration;
        transform.localPosition = v;
    }
}
