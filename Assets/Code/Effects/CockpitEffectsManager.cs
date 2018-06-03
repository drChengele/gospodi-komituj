using UnityEngine;

public class CockpitEffectsManager : MonoBehaviour {
    public void AddCockpitShake(float ferocity) {
        ObjectManager.Instance.PilotCamera.GetComponent<CameraShaker>().AddFerocity(ferocity);
    }
}