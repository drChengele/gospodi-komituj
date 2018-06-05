using UnityEngine;

public class WireColorizer : MonoBehaviour {

    public void SetWireColor(WireType wire) {
        GetComponent<MeshRenderer>().material = ObjectManager.Instance.WireSpawner.GetMaterialFor(wire);
    }

}
