using System;
using UnityEngine;

public interface IObjectManager { 
    IInertiaProvider InertiaSource { get; }
    Camera EngineeringCamera { get; }
    Camera PilotCamera { get; }
    Camera WorldCamera { get; }
    IShipController ShipController { get; }
    PilotController PilotController { get; }
    ShipSystems ShipSystems { get;  }
    GameManager GameManager { get; }
    CockpitEffectsManager CockpitEffects { get; }
    WireSpawner WireSpawner { get; }
    EngineeringController EngineeringController { get; }
    LayerMask WorldLayerMask { get; }
    PrefabContainer Prefabs { get; }
    CockpitController CockpitController { get; }
}


public class ObjectManager : MonoBehaviour, IObjectManager {

    [SerializeField] GameObject shipController;
    [SerializeField] PilotController pilotController;
    [SerializeField] Camera engineeringCamera;
    [SerializeField] Camera cockpitCamera;
    [SerializeField] Camera worldCamera;
    [SerializeField] ShipSystems shipSystems;
    [SerializeField] ShipReactor shipReactor;
    [SerializeField] GameManager gameManager;
    [SerializeField] CockpitEffectsManager cockpitEffects;
    [SerializeField] WireSpawner wireSpawner;
    [SerializeField] EngineeringController engineeringController;
    [SerializeField] LayerMask worldLayerMask;
    [SerializeField] PrefabContainer prefabs;
    [SerializeField] CockpitController cockpitController;

    public IInertiaProvider InertiaSource => shipController.GetComponent<IInertiaProvider>();
    public IShipController ShipController => shipController.GetComponent<IShipController>();
    public PilotController PilotController => pilotController;
    public Camera EngineeringCamera => engineeringCamera;
    public Camera PilotCamera => cockpitCamera;
    public Camera WorldCamera => worldCamera;
    public ShipSystems ShipSystems => shipSystems;
    public ShipReactor ShipReactor => shipReactor;
    public GameManager GameManager => gameManager;
    public CockpitEffectsManager CockpitEffects => cockpitEffects;
    public WireSpawner WireSpawner => wireSpawner;
    public EngineeringController EngineeringController => engineeringController;
    public LayerMask WorldLayerMask => worldLayerMask;
    public PrefabContainer Prefabs => prefabs;
    public CockpitController CockpitController => cockpitController;


    public static IObjectManager Instance { get; private set; }

    void Awake() {
        Instance = this;     
    }
}
