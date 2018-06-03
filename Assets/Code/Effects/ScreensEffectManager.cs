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
        var mm = FindObjectOfType<ShipMilageManager>();
        txt = txt.Replace("[DIST]", mm.remaining.ToString());
        txt = txt.Replace("[BOUNTY]", (mm.score * 13).ToString("$"));
        txt = txt.Replace("[WEAPONCHARGE]", $"{(int)(100 * ObjectManager.Instance.ShipSystems.Weapon.CurrentEnergy)}%" );
        txt = txt.Replace("[SHIELDCHARGE]", $"{(int)(100 * ObjectManager.Instance.ShipSystems.Shield.CurrentEnergy)}%");
        txt = txt.Replace("[ENGINECHARGE]", $"{(int)(100 * ObjectManager.Instance.ShipSystems.Engine.CurrentEnergy)}%");
        txt = txt.Replace("[REACTORTEMP]", "1 500 000 K");

        return txt;
    }

    void CycleRadarStatus() {
        currentRadarString = randomRadarStrings[UnityEngine.Random.Range(0, randomRadarStrings.Length)];
    }
    
    void UpdateRadarObjects() {
       
    }
}
