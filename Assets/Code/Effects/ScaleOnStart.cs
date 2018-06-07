using System;
using System.Collections.Generic;
using UnityEngine;


public class ScaleOnStart : MonoBehaviour, IRespondsToRespawn {

    [SerializeField] float scaleSpeed;
    [SerializeField] float scaleMin;
    [SerializeField] float scaleMax;

    bool isScaling;
    
    Vector3 _targetScale;

    public void OnRespawn() {
        StartScaling();
    }

    private void Awake() {
        StartScaling();
    }

    private void StartScaling() {
        isScaling = true;
        _targetScale = Vector3.one * UnityEngine.Random.Range(scaleMin, scaleMax);
        transform.localScale = Vector3.one * 0.001f;
    }

    private void Update() {
        if (!isScaling) return;
        transform.localScale = Vector3.MoveTowards(transform.localScale, _targetScale, scaleSpeed * Time.deltaTime);
        if ((transform.localScale - _targetScale).sqrMagnitude < 0.001f) {
            transform.localScale = _targetScale;
            isScaling = false;
        }
    }
}
