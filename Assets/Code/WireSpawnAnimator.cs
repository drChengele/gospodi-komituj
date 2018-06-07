using System;
using UnityEngine;

// used when wire is first spawned.
public class WireSpawnAnimator : MonoBehaviour {
    const float DampFactor = 0.25f;
    Vector3 vel;
    Vector3 targetPos;
    bool animationStarted;

    private void Awake() {
        targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update() {
        if (!animationStarted) return;

        if (transform.localPosition.z > 0.001f)
            MoveToZeroPlane();
        else {
            transform.localPosition = targetPos;
            GetComponent<Rigidbody>().isKinematic = false;
            Destroy(this);
        }
    }

    void MoveToZeroPlane() {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref vel, DampFactor);
    }

    internal void StartAnimation() {
        animationStarted = true;
    }
}