using System.Collections.Generic;
using UnityEngine;

public class RadarVisibleObject : MonoBehaviour {

    public float minDistance;
    public Color color;
    public int kind;

    internal Vector3 lastRelativePosition;

    public long GlobalIndex { get; private set; }

    static int _instanceCounter = 0;

    private void Awake() {
        GlobalIndex = ++_instanceCounter;
    }


}
