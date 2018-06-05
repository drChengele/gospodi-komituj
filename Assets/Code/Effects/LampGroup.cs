using System.Collections.Generic;
using UnityEngine;


public class LampGroup : MonoBehaviour {
    public GameObject[] lamps;
    public Vector3 individualOffset;
    [ColorUsage(false, true)]
    public Color noLightColor;
    [ColorUsage(false, true)]
    public Color lightColor;

    public void SetLevel(float zeroToOne) {
        var lastLamp = Mathf.RoundToInt( zeroToOne * lamps.Length) - 1;
        for (var i = 0; i < lamps.Length; i++) {
            lamps[i].GetComponent<MeshRenderer>().material.color = (i <= lastLamp) ? lightColor : noLightColor;
        }
    }

    private void Awake() {
        for (var i = 0; i < lamps.Length; i++) {
            lamps[i].transform.localPosition = individualOffset * i;
        }
        SetLevel(0.5f);
    }
}
