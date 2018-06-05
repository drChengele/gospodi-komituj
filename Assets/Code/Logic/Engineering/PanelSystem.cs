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

public abstract class InteractorTarget : ShipSystem {

}

public class PanelSystem : InteractorTarget {

    bool isCharging = false;
    public bool isPanel = true;

    [SerializeField] float rechargeRate;
    [SerializeField] DamageState currentDamageState;
    [SerializeField] Transform[] wireRoots;
    [SerializeField] LampGroup lamps;

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
        lamps.SetLevel(0f);
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
                lamps.SetLevel(0f);
                break;
            case DamageState.Malfunction:
                lamps.SetLevel(CurrentEnergy / 100f);
                DepletePanel();
                ChargePanel(isCharging);
                DeactivateCharging();
                if (CurrentEnergy < float.Epsilon || CurrentEnergy > MaxEnergy - float.Epsilon) {
                    ShortCircuit();
                }
                break;
            case DamageState.Destroyed:
                break;
            default:
                break;
        }
    }

    private void ShortCircuit() {
        //ObjectManager.Instance.GameManager
        ChangeDamageState(DamageState.Destroyed);
    }

    public void SlotWire(WireBehaviour wire) {
        var slot = GetFittingSlot(wire);
        slot.occupied = true;
        RegenerateSingleWireVisuals(slot);
        if (IsFullyRepaired()) {
            ChangeDamageState(DamageState.Operational);
        }
        Destroy(wire.gameObject);
    }

    public void ChangeDamageState(DamageState nextState) {

        if (nextState == currentDamageState) 
            return;

        currentDamageState = nextState;

        if (nextState == DamageState.Malfunction) {
            ObjectManager.Instance.GameManager.PanelBroken(this);
            RegenerateWireSlots();
            OpenGate();
        } 

        if (nextState == DamageState.Destroyed) {
            ObjectManager.Instance.GameManager.PanelDestroyed(this);
            RegenerateWireSlots();
        }

        if (nextState == DamageState.Operational) {
            ObjectManager.Instance.GameManager.PanelMadeOperational(this);
            CloseGate();
        }

    }

    private void CloseGate() {
        GetComponent<Animator>().SetBool("Open", false);
    }

    private void OpenGate() {
        GetComponent<Animator>().SetBool("Open", true);
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

    void RegenerateWireSlots() {
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
            SpawnObjectInWireSlot(wireSlot, 
                wireSlot.occupied ?  
                ObjectManager.Instance.WireSpawner.wirePrefabInSituFull :
                ObjectManager.Instance.WireSpawner.wirePrefabInSituMalfunction);
        }
    }

    private void SpawnObjectInWireSlot(WireSlot wireSlot, GameObject wirePrefab) {
        var wireGO = Instantiate(wirePrefab) as GameObject;
        wireGO.transform.parent = wireRoots[wireSlot.index];
        wireGO.transform.localPosition = Vector3.zero;
        wireGO.transform.localRotation = Quaternion.identity;
        wireGO.GetComponent<WireColorizer>()?.SetWireColor(wireSlot.wire);
        foreach (var cc in wireGO.GetComponentsInChildren<WireColorizer>()) cc.SetWireColor(wireSlot.wire);
    }

    private void DeleteWireVisuals(int index) {
        var z = wireRoots[index];
        foreach (Transform child in z.transform) GameObject.Destroy(child.gameObject);
    }
}
