using System;
using UnityEngine;

public class Canister : MonoBehaviour {
    public WireType wireType;

    private void Awake() {
        wireType = Utility.GetRandomWire();
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

static public partial class Utility {
    static public WireType GetRandomWire() {
        var diceRoll = UnityEngine.Random.Range(1, 4);
        if (diceRoll == 1) return WireType.Green;
        if (diceRoll == 2) return WireType.Orange;
        if (diceRoll == 3) return WireType.Purple;
        return WireType.Purple;
    }

    static public float ProjectNumbers(float fromMin, float fromMax, float toMin, float toMax, float value, bool clamp = true) {
        var t = Mathf.InverseLerp(fromMin, fromMax, value);
        if (clamp) t = Mathf.Clamp01(t);
        return Mathf.Lerp(toMin, toMax, t);
    }
}