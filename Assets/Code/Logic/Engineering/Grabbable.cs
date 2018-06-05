using System;
using UnityEngine;

public interface IEngineerInteractible {
    void OnHoldStarted();
    void OnHoldContinued();
    void OnHoldReleased();
}

[RequireComponent(typeof(Rigidbody))]
public abstract class EngineeringFloatingItem : MonoBehaviour {
    // different objects respond to inertia differently because why the fuck not.
    [NonSerialized] internal float receivedInertia;

    protected virtual void Awake() {
        receivedInertia = UnityEngine.Random.Range(0.8f, 1.2f);
    }
}

// a grabbable component object
public class Grabbable : EngineeringFloatingItem, IEngineerInteractible {
    const float RotationTime = 1f;    
    
    [SerializeField] Vector3 holdEuler;

    public event Action<Grabbable> HoldStarted;
    public event Action<Grabbable> HoldResumed;
    public event Action<Grabbable> HoldReleased;

    public Rigidbody Rigidbody => GetComponent<Rigidbody>();

    PanelSystem panelToCharge;

    float holdProgress = 0f; // 
    public bool isAligned => false;

    public void OnHoldContinued() {
        holdProgress += Time.deltaTime * RotationTime;
        Rigidbody.MoveRotation(Quaternion.Slerp(Rigidbody.transform.rotation, Quaternion.Euler(holdEuler), holdProgress));
        HoldResumed?.Invoke(this);
    }

    public void OnHoldReleased() {
        Rigidbody.isKinematic = false;
        holdProgress = 0f;
        DisableAllParticles();
        HoldReleased?.Invoke(this);
    }

    private void DisableAllParticles() {
        var ps = GetComponentInChildren<ParticleSystem>();
        ps?.Stop();        
    }

    private void EnableAllParticles() {
        var ps = GetComponentInChildren<ParticleSystem>();
        ps?.Play();
    }

    public void OnHoldStarted() {
        EnableAllParticles();
        Rigidbody.isKinematic = true;
        HoldStarted?.Invoke(this);
    }

    protected override void Awake() {
        base.Awake();
        DisableAllParticles();
    }
}
