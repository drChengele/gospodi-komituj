using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScreensEffectManager : MonoBehaviour {

    public GameObject radarScreen;
    public Text radarStatusText;

    [Serializable]
    public class RadarObjectDisplay {
        [ColorUsage(true, true)]public Color color;
        public float scale;
        public bool fixedScale;
        public bool blinking;
        public float detectionDistanceMultiplier;
        public Sprite icon;
    }

    [SerializeField] RadarObjectDisplay[] radarIcons;

    [SerializeField] string[] randomRadarStrings;
    [SerializeField] float cleanupFrequency;

    [SerializeField] int radarUpdateStep;

    string currentRadarString;

    private void Awake() {
        InvokeRepeating("CycleRadarStatus", 3f, 3f);
        InvokeRepeating("CleanupRadarObjects", cleanupFrequency, cleanupFrequency);
        CycleRadarStatus();
        foreach (var generator in FindAllGenerators()) {
            generator.ObjectGenerated += OnGeneratorObjectGenerated;
        }

    }

    IEnumerable<IObjectGenerator> FindAllGenerators() {
        return FindObjectsOfType<GameObject>().SelectMany(go => go.GetComponents<IObjectGenerator>());
    }

    private void OnGeneratorObjectGenerated(GameObject obj) {
        var rvo = obj.GetComponent<RadarVisibleObject>();
        if (rvo != null) trackedObjects.Add(rvo);
    }

    int framecounter = 0;

    private void Update() {
        UpdateRadarString();
        framecounter++;
        UpdateRadarObjectsSubset(radarUpdateStep, framecounter % radarUpdateStep);
    }

    private void UpdateRadarString() {
        radarStatusText.text = Process(currentRadarString);
        //radarStatusText.text = $"{trackedObjects.Count} objects tracked";
    }

    string Process(string radarString) {
        var txt = radarString;
        
        var mm = FindObjectOfType<ShipMilageManager>();
        if (mm != null ) {
            txt = txt.Replace("[DIST]", mm.remaining.ToString());
            txt = txt.Replace("[BOUNTY]", ((int)mm.Mileage * 13).ToString());
        }

        txt = txt.Replace("[TRACKED]", $"{trackedObjects.Count}");        
        txt = txt.Replace("[WEAPONCHARGE]", $"{(int)(ObjectManager.Instance.ShipSystems.Weapon.CurrentEnergy)}%" );
        txt = txt.Replace("[SHIELDCHARGE]", $"{(int)(ObjectManager.Instance.ShipSystems.Shield.CurrentEnergy)}%");
        txt = txt.Replace("[ENGINECHARGE]", $"{(int)(ObjectManager.Instance.ShipSystems.Engine.CurrentEnergy)}%");
        txt = txt.Replace("[REACTORTEMP]", "1 500 000 K");

        return txt;
    }

    void CycleRadarStatus() {
        currentRadarString = randomRadarStrings[UnityEngine.Random.Range(0, randomRadarStrings.Length)];
    }

    void UpdateRadarObjectsSubset(int step, int offset) {
        if (trackedObjectsArray == null)
            return;

        for (var i = offset; i < trackedObjectsArray.Length; i+= step ) {
            AddOrUpdateObjectGraphic(trackedObjectsArray[i]);
        }
    }

    void CleanupRadarObjects() {
        var toRemove = trackedObjects.Where(obj => obj == null);
        foreach (var item in toRemove) RemoveObjectGraphic(item);
        trackedObjects.RemoveWhere(obj => obj == null);
        trackedObjectsArray = trackedObjects.ToArray();
    }

    Dictionary<RadarVisibleObject, RadarBlip> maintainedGraphics = new Dictionary<RadarVisibleObject, RadarBlip>();

    void AddOrUpdateObjectGraphic(RadarVisibleObject radarVisibleObject) {
        if (radarVisibleObject == null) return;
        RadarBlip blip;
        if (!maintainedGraphics.TryGetValue(radarVisibleObject, out blip)) {
            var go = Instantiate(ObjectManager.Instance.Prefabs.radarImage);
            go.transform.parent = radarScreen.transform;
            blip = go.GetComponent<RadarBlip>();
            maintainedGraphics[radarVisibleObject] = blip;
            blip.AddToRadar(radarVisibleObject, radarIcons[radarVisibleObject.kind]);            
        }

        float mul = radarIcons[radarVisibleObject.kind].detectionDistanceMultiplier;
        var relativePosition = ObjectManager.Instance.ShipController.GameObject.transform.InverseTransformPoint(radarVisibleObject.transform.position);
        relativePosition /= mul;
        radarVisibleObject.lastRelativePosition = relativePosition;

        blip.transform.localScale = Vector3.one;
        var enabled = ShouldBeVisible(radarVisibleObject);
        blip.SpriteRenderer.enabled = enabled;
        if (enabled) {
            var data = radarIcons[radarVisibleObject.kind];
            float xFactor = 4f;
            // simulate enlargement through distance
            var dist = relativePosition.z;
            xFactor *= Utility.ProjectNumbers(0, visibilityDistanceAhead * mul, 1.2f, 0.2f, dist);
            blip.transform.localPosition = new Vector3(relativePosition.x, relativePosition.y, 0f) * xFactor / visibilitySpanHorizontal;
            var scale = Vector3.one * data.scale;
            if (!data.fixedScale) scale *= 1f - relativePosition.z / visibilityDistanceAhead;
            blip.transform.localScale = scale;
        }
    }

    bool ShouldBeVisible(RadarVisibleObject item) {

        float mul = radarIcons[item.kind].detectionDistanceMultiplier;

        return Mathf.Abs(item.lastRelativePosition.x) < visibilitySpanHorizontal * mul
            && Mathf.Abs(item.lastRelativePosition.y) < visibilitySpanHorizontal * mul
            && item.lastRelativePosition.z < visibilityDistanceAhead * mul
            && item.lastRelativePosition.z > 0f;

    }

    [SerializeField] float visibilitySpanHorizontal;
    [SerializeField] float visibilityDistanceAhead;

    void RemoveObjectGraphic(RadarVisibleObject item) {
        RadarBlip blip;
        if (maintainedGraphics.TryGetValue(item, out blip)) {
            Destroy(blip.gameObject);
            maintainedGraphics.Remove(item);
        }
    }

    [SerializeField] ObjectGenerator generator;

    HashSet<RadarVisibleObject> trackedObjects = new HashSet<RadarVisibleObject>();
    RadarVisibleObject[] trackedObjectsArray;
}
