using System;
using UnityEngine;

public interface IObjectManager { 
    IInertiaProvider InertiaSource { get; }
    Camera EngineeringCamera { get; }
    Camera PilotCamera { get; }
    Camera WorldCamera { get; }
}


public class ObjectManager : MonoBehaviour, IObjectManager {

    [SerializeField] JamShipController shipController;
    [SerializeField] Camera engineeringCamera;
    [SerializeField] Camera cockpitCamera;
    [SerializeField] Camera worldCamera;

    public IInertiaProvider InertiaSource => shipController;
    public Camera EngineeringCamera => engineeringCamera;
    public Camera PilotCamera => cockpitCamera;
    public Camera WorldCamera => worldCamera;

    public static IObjectManager Instance { get; private set; }

    void Awake() {
        Instance = this;
        FindReferences();
    }

    private void FindReferences() {
        if (shipController == null) shipController = FindObjectOfType<JamShipController>();
    }

}
