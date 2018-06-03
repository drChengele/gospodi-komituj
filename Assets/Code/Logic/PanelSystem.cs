using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DamageState {
    Operational,
    Malfunction,
    Destroyed,
}

public class PanelSystem : ShipSystem , IEngineerInteractible {

    [SerializeField] float rechargeRate;
    bool isCharging = false;
    public bool isPanel = true;

    [SerializeField] DamageState currentDamageState;
    [SerializeField] Transform[] wireRoots;

    public DamageState CurrentDamageState => currentDamageState;

    public class WireSlot {
        public readonly WireType wire;
        public readonly int index;
        public bool occupied;
        
        public WireSlot(WireType wire, int index) {
            this.wire = wire;
            this.occupied = false;
            this.index = index;
        }
    }

    WireSlot[] wireSlots;

    private void Awake()
    {
        this.DefaultEnergy(50f);
        //if (currentDamageState == null) currentDamageState = DamageState.Operational;
    }

    public void ChargePanel(bool charge)
    {
        if (charge) this.TryChangeCurrentEnergy(rechargeRate * Time.deltaTime);
    }

    void DepletePanel()
    {
        if (this.CurrentEnergy > 0f)
        {
            this.TryChangeCurrentEnergy(-energyDepletionRate * Time.deltaTime);
        }
    }

    public void ActivateCharging()
    {
        if (currentDamageState == DamageState.Malfunction && !isCharging) isCharging = true;
    }

    public void DeactivateCharging()
    {
        isCharging = false;
    }

    private void Update()
    {
        switch (currentDamageState)
        {
            case DamageState.Operational:
                break;
            case DamageState.Malfunction:
                DepletePanel();
                ChargePanel(isCharging);
                DeactivateCharging();
                Debug.Log(this.CurrentEnergy);
                break;
            case DamageState.Destroyed:
                break;
            default:
                break;
        }
    }

    public void OnHoldStarted()
    {
        //throw new System.NotImplementedException();
    }

    public void OnHoldContinued()
    {
        //throw new System.NotImplementedException();
    }

    public void OnHoldReleased()
    {
       // throw new System.NotImplementedException();
    }

    public void SlotWire(WireBehaviour wire) {
        var slot = GetFittingSlot(wire);
        slot.occupied = true;
        RegenerateSingleWireVisuals(slot);
        if (IsFullyRepaired()) {
            ChangeDamageState(DamageState.Operational);
        }
    }

    public void ChangeDamageState(DamageState nextState) {

        if (nextState == currentDamageState) 
            return;

        if (nextState == DamageState.Malfunction) {
            ObjectManager.Instance.GameManager.PanelBroken(this);
            GenerateWireSlots();
            OpenGate();
        } 

        if (nextState == DamageState.Destroyed) {
            ObjectManager.Instance.GameManager.PanelDestroyed(this);
        }

        if (nextState == DamageState.Operational) {
            ObjectManager.Instance.GameManager.PanelMadeOperational(this);
            CloseGate();
        }

        currentDamageState = nextState;
    }

    private void CloseGate() {
        // todo : animation
    }

    private void OpenGate() {
        // todo : animation
    }

    private bool IsFullyRepaired() {
        return CurrentDamageState == DamageState.Malfunction && wireSlots.All(slot => slot.occupied);
    }

    public bool CanSlotWire(WireBehaviour wire) {
        var fittingSlot = GetFittingSlot(wire);
        return fittingSlot != null;
    }

    private WireSlot GetFittingSlot(WireBehaviour wire) => wireSlots
                .Where(slot => !slot.occupied)
                .FirstOrDefault(slot => slot.wire == wire.wType);

    void GenerateWireSlots() {
        if(wireSlots != null) foreach (var slot in wireSlots) DeleteWireVisuals(slot.index);

        wireSlots = new WireSlot[3];
        for (var i = 0; i < 3; i++) {
            wireSlots[i] = new WireSlot(Utility.GetRandomWire(), i);
            RegenerateSingleWireVisuals(wireSlots[i]);
        }
    }

    void RegenerateSingleWireVisuals(WireSlot wireSlot) {
        DeleteWireVisuals(wireSlot.index);
        if (currentDamageState == DamageState.Operational) {
            SpawnObjectInWireSlot(wireSlot, ObjectManager.Instance.WireSpawner.wirePrefabInSituFull);
        }
        if (currentDamageState == DamageState.Destroyed) {
            SpawnObjectInWireSlot(wireSlot, ObjectManager.Instance.WireSpawner.wirePrefabInSituDestroyed);
        }
        if (currentDamageState == DamageState.Malfunction) {
            SpawnObjectInWireSlot(wireSlot, ObjectManager.Instance.WireSpawner.wirePrefabInSituMalfunction);
        }
    }

    private void SpawnObjectInWireSlot(WireSlot wireSlot, GameObject wirePrefab) {
        var wireGO = Instantiate(wirePrefab) as GameObject;
    }



    private void DeleteWireVisuals(int index) {
        var z = wireRoots[index];
        foreach (Transform child in z.transform) GameObject.Destroy(child.gameObject);
    }
}
