using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonStuff : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler
{
    [SerializeField] GameObject glow;
    GlowToggler glowToggler;

    void Awake()
    {
        glowToggler = glow.GetComponent<GlowToggler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        glowToggler.ToggleVisibility();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        glowToggler.ToggleVisibility();
    }
}
