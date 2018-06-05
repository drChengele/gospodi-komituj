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
        generator.ObjectGenerated += OnGeneratorObjectGenerated;
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
        txt = txt.Replace("[TRACKED]", $"{trackedObjects.Count} objects tracked");
        txt = txt.Replace("[DIST]", mm.remaining.ToString());
        txt = txt.Replace("[BOUNTY]", ((int)mm.Mileage * 13).ToString());
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

    Dictionary<RadarVisibleObject, SpriteRenderer> maintainedGraphics = new Dictionary<RadarVisibleObject, SpriteRenderer>();

    void AddOrUpdateObjectGraphic(RadarVisibleObject radarVisibleObject) {
        if (radarVisibleObject == null) return;
        SpriteRenderer graphic;
        if (!maintainedGraphics.TryGetValue(radarVisibleObject, out graphic)) {
            var go = Instantiate(ObjectManager.Instance.Prefabs.radarImage);
            go.transform.parent = radarScreen.transform;
            maintainedGraphics[radarVisibleObject] = graphic = go.GetComponent<SpriteRenderer>();
            //graphic.transform.localRotation = Quaternion.Euler(0, 0, 180);
            graphic.color = radarIcons[radarVisibleObject.kind].color;
            graphic.sprite = radarIcons[radarVisibleObject.kind].icon;
        }        
        var relativePosition = ObjectManager.Instance.ShipController.transform.InverseTransformPoint(radarVisibleObject.transform.position);
        radarVisibleObject.lastRelativePosition = relativePosition;

        graphic.transform.localScale = Vector3.one;
        graphic.enabled = ShouldBeVisible(radarVisibleObject);
        if (graphic.enabled) {
            var data = radarIcons[radarVisibleObject.kind];
            float xFactor = 4f;
            // simulate enlargement through distance
            var dist = relativePosition.z;
            xFactor *= Utility.ProjectNumbers(0, visibilityDistanceAhead, 1.2f, 0.2f, dist);
            graphic.transform.localPosition = new Vector3(relativePosition.x, relativePosition.y, 0f) * xFactor / visibilitySpanHorizontal;            
            graphic.transform.localScale = Vector3.one * data.scale * (1f - relativePosition.z / visibilityDistanceAhead);
        }
    }

    bool ShouldBeVisible(RadarVisibleObject item) {
        return Mathf.Abs(item.lastRelativePosition.x) < visibilitySpanHorizontal
            && Mathf.Abs(item.lastRelativePosition.y) < visibilitySpanHorizontal
            && item.lastRelativePosition.z < visibilityDistanceAhead
            && item.lastRelativePosition.z > 0f;

    }

    [SerializeField] float visibilitySpanHorizontal;
    [SerializeField] float visibilityDistanceAhead;

    void RemoveObjectGraphic(RadarVisibleObject item) {
        SpriteRenderer sr;
        if (maintainedGraphics.TryGetValue(item, out sr)) {
            Destroy(sr.gameObject);
            maintainedGraphics.Remove(item);
        }
    }

    [SerializeField] ObjectGenerator generator;

    HashSet<RadarVisibleObject> trackedObjects = new HashSet<RadarVisibleObject>();
    RadarVisibleObject[] trackedObjectsArray;
}
