using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScreensEffectManager : MonoBehaviour {

    public GameObject radarScreen;
    public Text radarStatusText;

    [SerializeField] string[] randomRadarStrings;
    [SerializeField] float radarFrequency;

    string currentRadarString;

    private void Awake() {
        InvokeRepeating("CycleRadarStatus", 3f, 3f);
        InvokeRepeating("UpdateRadarObjects", radarFrequency, radarFrequency);
        CycleRadarStatus();
    }
    private void Update() {
        UpdateRadar();
    }

    private void UpdateRadar() {
        radarStatusText.text = Process(currentRadarString);
    }

    string Process(string radarString) {
        var txt = radarString;
        txt = txt.Replace("[DIST]", "123");
        txt = txt.Replace("[BOUNTY]", "4983242$$");
        txt = txt.Replace("[WEAPONCHARGE]", "58.5%");
        txt = txt.Replace("[SHIELDCHARGE]", "58.5%");
        txt = txt.Replace("[ENGINECHARGE]", "58.5%");
        txt = txt.Replace("[REACTORTEMP]", "1 500 000 K");

        return txt;
    }

    void CycleRadarStatus() {
        currentRadarString = randomRadarStrings[UnityEngine.Random.Range(0, randomRadarStrings.Length)];
    }
    
    void UpdateRadarObjects() {
       
    }
}
