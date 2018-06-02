using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with engineer mouse UI (interacting with engineering stuff : buttons, sliders, tools and debris
/// Engineer can CLICK buttons
/// Engineer can GRAB, DRAG and THROW debris and tools
/// </summary>
public class EngineeringController : MonoBehaviour {

    [SerializeField] Transform junkLayerRoot; // this is where tools and debris float
    [SerializeField] [Range(0f, 2f)] float retainShipInertia;
    [SerializeField] LayerMask interactiveLayerMask;
    [SerializeField] float maxObjectDragExtentsHorizontal;
    [SerializeField] float maxObjectDragExtentsVertical;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            MouseHoldStarted();
        } else if (Input.GetMouseButtonUp(0)) {
            MouseReleased();
        } else if (Input.GetMouseButton(0)) {
            MouseHeld();
        }
    }

    private void FixedUpdate() {
        ProcessJunkPhysics();
    }

    private void ProcessJunkPhysics() {
        var allObjects = junkLayerRoot.GetComponentsInChildren<Grabbable>();
        var acceleration = ObjectManager.Instance.InertiaSource.CurrentAccelerationRelative * -retainShipInertia;
        foreach (var grabbable in allObjects) grabbable.Rigidbody.AddForce(acceleration * grabbable.receivedInertia, ForceMode.Acceleration);
    }
    private void UpdatePositionOfGrabbed() {
        // find intersection of current mouse ray with engineering junk layer plane 
        var intersect = GetMouseIntersectionPointWithJunkPlane();

        if (intersect != null && previousIntersectWithPlane != null) {
            var delta = intersect.Value - previousIntersectWithPlane.Value;
            var targetPosition = grabbed.transform.position + delta;
            targetPosition = ConstrainTargetPositionToBeInsideTheJunkRectangle(targetPosition);
            grabbed.Rigidbody.MovePosition(targetPosition);
        }

        previousIntersectWithPlane = intersect;

    }

    // so you don't drag outside the desired radius
    Vector3 ConstrainTargetPositionToBeInsideTheJunkRectangle(Vector3 position) {
        position.x = Mathf.Clamp(position.x, -maxObjectDragExtentsHorizontal, maxObjectDragExtentsHorizontal);
        position.y = Mathf.Clamp(position.y, -maxObjectDragExtentsVertical, maxObjectDragExtentsVertical);
        return position;
    }

    Vector3? GetMouseIntersectionPointWithJunkPlane() {
        var plane = new Plane(-junkLayerRoot.forward, junkLayerRoot.position);
        float where;
        var ray = ObjectManager.Instance.EngineeringCamera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out where)) {
            return ray.origin + ray.direction * where;
        }
        return null;
    }

    Vector3? previousIntersectWithPlane;

    private void MouseReleased() {
        grabbed?.OnHoldReleased();
        grabbed = null;
        previousIntersectWithPlane = null;
    }

    Grabbable grabbed;

    private void MouseHoldStarted() {
        var ray = ObjectManager.Instance.EngineeringCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 10f, interactiveLayerMask)) {
            var interactive = hitinfo.collider.gameObject.GetComponent<IEngineerInteractible>() ?? hitinfo.rigidbody?.gameObject.GetComponent<IEngineerInteractible>();
            if (interactive != null) {
                interactive.OnHoldStarted();
                grabbed = interactive as Grabbable;
            }
        }
    }

    private void MouseHeld() {
        if (grabbed != null) {
            UpdatePositionOfGrabbed();
            grabbed.OnHoldContinued();
        }
        else
        {
            var ray = ObjectManager.Instance.EngineeringCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 10f, interactiveLayerMask))
            {
                var interactive = hitinfo.collider.gameObject.GetComponent<IEngineerInteractible>() ?? hitinfo.rigidbody?.gameObject.GetComponent<IEngineerInteractible>();
                var button = interactive as EngineeringButton;

                if (button != null)
                {
                    button.OnHoldContinued();
                }
            }
        }
    }

}