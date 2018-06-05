using System.Collections.Generic;
using UnityEngine;

public class ScaleOnStart : MonoBehaviour {

    [SerializeField] float time;
    
    Vector3 _targetScale;

    private void Awake() {
        _targetScale = transform.localScale;
        transform.localScale = Vector3.one * 0.001f;
    }

    private void Update() {
        transform.localScale = Vector3.MoveTowards(transform.localScale, _targetScale, time * Time.deltaTime);
        if ((transform.localScale - _targetScale).sqrMagnitude < 0.001f) {
            transform.localScale = _targetScale;
            Destroy(this);
        }
    }
}
