using System;
using UnityEngine;

public interface IObjectManager { 
    IInertiaProvider InertiaSource { get; }
    Camera EngineeringCamera { get; }
    Camera PilotCamera { get; }
    Camera WorldCamera { get; }
    JamShipController ShipController { get; }
    PilotController PilotController { get; }
    ShipSystems ShipSystems { get;  }
    GameManager GameManager { get; }
    CockpitEffectsManager CockpitEffects { get; }
    WireSpawner WireSpawner { get; }
}


public class ObjectManager : MonoBehaviour, IObjectManager {

    [SerializeField] JamShipController shipController;
    [SerializeField] PilotController pilotController;
    [SerializeField] Camera engineeringCamera;
    [SerializeField] Camera cockpitCamera;
    [SerializeField] Camera worldCamera;
    [SerializeField] ShipSystems shipSystems;
    [SerializeField] ShipReactor shipReactor;
    [SerializeField] GameManager gameManager;
    [SerializeField] CockpitEffectsManager cockpitEffects;
    [SerializeField] WireSpawner wireSpawner;

    public IInertiaProvider InertiaSource => shipController;
    public JamShipController ShipController => shipController;
    public PilotController PilotController => pilotController;
    public Camera EngineeringCamera => engineeringCamera;
    public Camera PilotCamera => cockpitCamera;
    public Camera WorldCamera => worldCamera;
    public ShipSystems ShipSystems => shipSystems;
    public ShipReactor ShipReactor => shipReactor;
    public GameManager GameManager => gameManager;
    public CockpitEffectsManager CockpitEffects => cockpitEffects;
    public WireSpawner WireSpawner => wireSpawner;


    public static IObjectManager Instance { get; private set; }

    void Awake() {
        Instance = this;
        FindReferences();
    }

    private void FindReferences() {
        if (shipController == null) shipController = FindObjectOfType<JamShipController>();
    }

}
