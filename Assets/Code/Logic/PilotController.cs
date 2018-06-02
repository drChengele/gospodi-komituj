﻿using UnityEngine;

// processes input and communicates the signals
public class PilotController : MonoBehaviour {
    internal Vector2 thrusterSignal;
    internal bool    fireSignal;
    internal bool    shieldSignal;

    private void Update() {
        thrusterSignal = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (thrusterSignal.magnitude > 1f) thrusterSignal.Normalize();
        fireSignal = Input.GetButton("Fire");
        shieldSignal = Input.GetButton("Shield");
    }
}