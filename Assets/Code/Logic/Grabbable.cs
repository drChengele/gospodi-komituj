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
    [SerializeField] Transform rayOrigin;


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
            Rigidbody.MoveRotation(Quaternion.Slerp(Rigidbody.transform.rotation, Quaternion.Euler(90f, 0f, 0f), timeFraction));
            if (toolAligned)
            {
                RaycastHit hit;
                //Debug.DrawLine(rayOrigin.position, rayOrigin.up * 10f,Color.red,2f);
                if (Physics.Raycast(rayOrigin.position, rayOrigin.up * 10f, out hit, layerMask))
                {
                    var panel = hit.collider.gameObject.GetComponent<IEngineerInteractible>() ?? hit.rigidbody?.gameObject.GetComponent<IEngineerInteractible>();
                    if (panel != null)
                    {
                        panelToCharge = panel as PanelSystem;
                        panelToCharge?.ActivateCharging();
                    }
                }
                panelToCharge = null;
            }
        }
    }

    public void OnHoldReleased() {
        Rigidbody.isKinematic = false;
        if (isTool) ResetToolData();
    }

    public void OnHoldStarted() {
        Rigidbody.isKinematic = true;
    }

    private void Awake() {
        receivedInertia = Random.Range(0.8f, 1.2f);
    }

    private void ResetToolData()
    {
        elapsedTime = 0f;
        toolAligned = false;
    }
}
