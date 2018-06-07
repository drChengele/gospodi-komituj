using System.Collections.Generic;
using UnityEngine;

public interface IRespondsToRespawn {
    void OnRespawn();
}

public class FudgedInfiniteObject : MonoBehaviour {

    FudgedInfiniteObjectField myField;
    IRespondsToRespawn[] allResponders;

    public void Initialize(FudgedInfiniteObjectField field) {
        myField = field;
        allResponders = GetComponents<IRespondsToRespawn>();
    }

    public void Respawn() {
        gameObject.SetActive(true);
        foreach (var responder in allResponders) responder.OnRespawn();
    }

}

