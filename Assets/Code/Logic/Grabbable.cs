using UnityEngine;

public interface IEngineerInteractible {
    void OnHoldStarted();
    void OnHoldContinued();
    void OnHoldReleased();
}

// a grabbable component object
public class Grabbable : MonoBehaviour, IEngineerInteractible {
    // different objects respond to inertia differently because why the fuck not.
    [System.NonSerialized] public float receivedInertia;

    public Rigidbody Rigidbody => GetComponent<Rigidbody>();

    public void OnHoldContinued() { 

    }

    public void OnHoldReleased() {
        Rigidbody.isKinematic = false;
    }

    public void OnHoldStarted() {
        Rigidbody.isKinematic = true;
    }

    private void Awake() {
        receivedInertia = Random.Range(0.8f, 1.2f);
    }
}
