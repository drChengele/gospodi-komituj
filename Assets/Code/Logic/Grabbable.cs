using System;
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
    [SerializeField] bool isTool = false;
    [SerializeField] float rotationTime = 0f;
    float elapsedTime = 0f;
    bool toolAligned = false;
    [SerializeField] LayerMask layerMask;
    [SerializeField] public Transform rayOrigin;
    [SerializeField] Vector3 holdEuler;
    [SerializeField] bool isCharger;


    public Rigidbody Rigidbody => GetComponent<Rigidbody>();

    PanelSystem panelToCharge;

    public void OnHoldContinued() {
        if (isTool)
        {
            elapsedTime += Time.deltaTime;
            var timeFraction = elapsedTime / rotationTime;
            if (timeFraction >= 1f)
            {
                timeFraction = 1f;
                toolAligned = true;
            }
            Rigidbody.MoveRotation(Quaternion.Slerp(Rigidbody.transform.rotation, Quaternion.Euler(holdEuler), timeFraction));
            if (isCharger) {
                var panelToCharge = GetPanelUnderneath();
                panelToCharge?.ActivateCharging();
                panelToCharge = null;
            }
            
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(rayOrigin.position.normalized, rayOrigin.up);
    }

    public PanelSystem GetPanelUnderneath() {
        var all = Physics.RaycastAll(rayOrigin.position, rayOrigin.up, 10f, layerMask) ;

        foreach (var hit in all) {
            var panel = hit.collider.transform.parent.gameObject.GetComponent<PanelSystem>();
            if (panel != null) {
                Debug.Log($"Found panel {panel.name}");
                return panel;
            }
        }
        return null;
        
    }

    public void OnHoldReleased() {
        Rigidbody.isKinematic = false;
        if (isTool) {
            ResetToolData();
            var wire = GetComponent<WireBehaviour>();
            wire?.ProcessReleased();
        }
        DisableAllParticles();
    }

    private void DisableAllParticles() {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Stop();
    }

    private void EnableAllParticles() {
        var ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    public void OnHoldStarted() {
        EnableAllParticles();
        Rigidbody.isKinematic = true;
    }


    private void Awake() {
        receivedInertia = UnityEngine.Random.Range(0.8f, 1.2f);
        DisableAllParticles();
    }

    private void ResetToolData()
    {
        elapsedTime = 0f;
        toolAligned = false;
    }
}
