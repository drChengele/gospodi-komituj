using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowToggler : MonoBehaviour {

    public void ToggleVisibility()
    {
        if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
