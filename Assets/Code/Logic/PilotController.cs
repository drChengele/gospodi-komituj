using UnityEngine;

// processes input and communicates the signals
public class PilotController : MonoBehaviour {
    internal Vector2 thrusterSignal;
    internal float rollSignal;
    internal bool    fireSignal;
    internal bool    shieldSignal;
    internal float forwardSignal;

    private void Update() {
        thrusterSignal = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rollSignal = Input.GetAxis("Roll");        
        if (thrusterSignal.magnitude > 1f) thrusterSignal.Normalize();
        fireSignal = Input.GetButton("Fire");
        shieldSignal = Input.GetButton("Shield");
        forwardSignal = Input.GetAxis("Forward");
    }
}