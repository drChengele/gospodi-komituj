using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingUIElement : MonoBehaviour {
    public bool ShouldBeActivated = true;
    public bool IsBlinking = true;
    Graphic _beh;
    [SerializeField] public float period;
    [SerializeField] [Range(0.1f, 0.9f)] public float fraction = 0.2f;

    private void Awake() {
        _beh = GetComponent<Graphic>();
    }

    private void Update() {
        _beh.enabled =_Active();
    }

    bool _Active() {
        return ShouldBeActivated && (IsBlinking ? (Time.time % period < period * fraction) : true);
    }
}
