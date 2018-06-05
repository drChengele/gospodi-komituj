using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour {

    [SerializeField] float sinewaveIntensity;
    [SerializeField] float sinewavePeriod = 1f;

    float initLevel;

    private void Awake() {
        initLevel = GetComponent<Light>().intensity;
    }

    private void Update() {
        var currentIntensity = 1f + Mathf.Sin(Time.time * Mathf.PI * 2 / sinewavePeriod) * sinewaveIntensity;
        GetComponent<Light>().intensity = currentIntensity;
    }

}
