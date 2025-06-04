using UnityEngine;

public class CableAnchor : MonoBehaviour {
    public GameObject Ghost;
    public GameObject ColorIndicator;
    public Material MatOn;
    public Material MatOff;
    public bool CablePlugged;
    public bool CableOk;

    private void Start() {
        InitializeComponents();
    }

    private void InitializeComponents() {
        DeactivateGhost();
        CableOk = false;
    }

    public void VerifyColor(Material cableMat) {
        if (cableMat.name == MatOn.name) SetMaterialStatus(true, MatOn);
        else SetMaterialStatus(false, MatOff);
    }

    private void SetMaterialStatus(bool status, Material material) {
        CableOk = status;
        Debug.Log($"{material.name}, {MatOn.name}, {CableOk}");
        ColorIndicator.GetComponent<MeshRenderer>().material = material;
    }

    public void ColorOff() {
        SetMaterialStatus(false, MatOff);
    }

    public void ActivateGhost() {
        if (!Ghost.activeSelf) Ghost.SetActive(true);
    }

    public void DeactivateGhost() {
        Ghost.SetActive(false);
    }

    public void SetCablePlugged(bool status) {
        CablePlugged = status;
    }
}