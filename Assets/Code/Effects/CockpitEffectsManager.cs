using UnityEngine;

public class CockpitEffectsManager : MonoBehaviour {
    public void AddCockpitShake(float ferocity) {
        ObjectManager.Instance.PilotCamera.GetComponent<CameraShaker>().AddFerocity(ferocity);
        ObjectManager.Instance.WorldCamera.GetComponent<CameraShaker>().AddFerocity(ferocity);
        ObjectManager.Instance.EngineeringCamera.GetComponent<CameraShaker>().AddFerocity(ferocity);
    }
}