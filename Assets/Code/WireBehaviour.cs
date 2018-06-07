using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WireType
{
    Purple,
    Orange,
    Green,
}

[RequireComponent(typeof(Grabbable))]
public class WireBehaviour : MonoBehaviour {
    
    public WireType wireType { get; private set; }
    // Use this for initialization
    void Awake () {
        GetComponent<Grabbable>().HoldReleased += OnThisWasReleased;
	}

    void OnThisWasReleased(Grabbable grabbable) {
        var panel = grabbable.GetComponent<Interactor>()?.GetInteractorTargetUnderneathMe() as PanelSystem;
        if (panel != null) ObjectManager.Instance.GameManager.AttemptedWireSlotting(this, panel);
    }

    internal void SetWireType(WireType type) {
        wireType = type;
        (GetComponent<WireColorizer>() ?? GetComponentInChildren<WireColorizer>())?.SetWireColor(wireType);
    }
}