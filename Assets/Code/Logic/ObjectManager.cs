using System;
using UnityEngine;

public interface IObjectManager { 
    IInertiaProvider InertiaSource { get; }
}


public class ObjectManager : MonoBehaviour, IObjectManager {

    [SerializeField] JamShipController shipController;

    public IInertiaProvider InertiaSource => shipController;

    public static IObjectManager Instance { get; private set; }

    void Awake() {
        Instance = this;
        FindReferences();
    }

    private void FindReferences() {
        if (shipController == null) shipController = GameObject.FindObjectOfType<JamShipController>();
    }
}
