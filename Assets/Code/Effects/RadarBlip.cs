using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class RadarBlip : MonoBehaviour {
    public SpriteRenderer SpriteRenderer { get; private set; }
    public bool blinking;

    RadarVisibleObject myObject;

    public void AddToRadar(RadarVisibleObject sourceObject, ScreensEffectManager.RadarObjectDisplay icon ) {
        myObject = sourceObject;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = icon.color;
        SpriteRenderer.sprite = icon.icon;
    }
}
