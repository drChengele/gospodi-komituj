using System.Collections.Generic;
using UnityEngine;


public class ElectronicsEffectsController : MonoBehaviour {
    [SerializeField] float distortionLineMoveSpeed;
    private OLDTVScreen screen;
    private OLDTVTube tube;

    private void Awake() {
        screen = GetComponent<OLDTVScreen>();
        tube = GetComponent<OLDTVTube>();
    }

    private void Update() {
        screen.staticVertical += distortionLineMoveSpeed * Time.deltaTime;
    }
}
