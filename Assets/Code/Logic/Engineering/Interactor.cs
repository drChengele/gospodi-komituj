
using UnityEngine;

public class Interactor : MonoBehaviour {
    [SerializeField] LayerMask interactionTargetLayers;
    [SerializeField] Transform raycaster;

    public InteractorTarget GetInteractorTargetUnderneathMe() {
        var all = Physics.RaycastAll(raycaster.position, raycaster.forward, 10f, ObjectManager.Instance.EngineeringController.interactionLayers);
        foreach (var hit in all) {
            var target = hit.collider.transform.parent.gameObject.GetComponent<InteractorTarget>();
            if (target != null) {
                Debug.Log($"Interactor target found: {target.name}");
            }
            return target;
        }
        return null;
    }
}
