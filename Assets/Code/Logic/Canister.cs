using System;
using UnityEngine;

public class Canister : MonoBehaviour {
    public WireType wireType;

    private void Awake() {
        var diceRoll = UnityEngine.Random.Range(1, 4);
        if (diceRoll == 1) wireType = WireType.Green;
        if (diceRoll == 2) wireType = WireType.Orange;
        if (diceRoll == 3) wireType = WireType.Purple;
        UpdateMaterialColor();
    }

    private void UpdateMaterialColor() {
        var material = GetComponent<MeshRenderer>().material;
        material.color = GetColorFromWire(wireType);
    }

    Color GetColorFromWire(WireType wire) {
        switch (wire) {
            case WireType.Purple: return new Color(1f, 0.4f, 1f);
            case WireType.Orange: return new Color(1f, 0.8f, 0.2f);
            case WireType.Green: return new Color(0.4f, 1f, 0.34f);
        }
        return Color.black;
    }
}