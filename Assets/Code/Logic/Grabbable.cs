using UnityEngine;

// a grabbable component object
public class Grabbable : MonoBehaviour {
    // different objects respond to inertia differently because why the fuck not.
    [System.NonSerialized] public float receivedInertia;

    public Rigidbody Rigidbody => GetComponent<Rigidbody>();

    private void Awake() {
        receivedInertia = Random.Range(0.8f, 1.2f);
    }
}
