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
            if (toolAligned)
            {
                var panelToCharge = GetPanelUnderneath();
                panelToCharge?.ActivateCharging();
                panelToCharge = null;
            }
        }
    }

    public PanelSystem GetPanelUnderneath() {
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.up , out hit, 10f, layerMask)) {
            //var panel = hit.collider.gameObject.GetComponent<PanelSystem>() ?? hit.rigidbody?.gameObject.GetComponent<PanelSystem>();
            //return panel;
            var panel = hit.collider.transform.parent.gameObject.GetComponent<PanelSystem>();
            return panel;
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
    }

    public void OnHoldStarted() {
        Rigidbody.isKinematic = true;
    }

    private void Awake() {
        receivedInertia = UnityEngine.Random.Range(0.8f, 1.2f);
    }

    private void ResetToolData()
    {
        elapsedTime = 0f;
        toolAligned = false;
    }
}
