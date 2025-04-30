using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Fusible : MonoBehaviour {
    private bool _anchorDetected;
    private Transform _anchorTransform;
    private XRGrabInteractable _grabInteractable;
    public MeshRenderer GhostMeshRenderer;
    private Rigidbody _rb;
    public bool FusibleOk;

    private void Start() {
        GhostMeshRenderer.enabled = false;
        InitializeComponents();
        SetupEventListeners();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Anchor")) {
            HandleAnchorExit(other);
            if (GhostMeshRenderer.enabled)GhostMeshRenderer.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Anchor")) {
            HandleAnchorStay(other);
            if (!GhostMeshRenderer.enabled)GhostMeshRenderer.enabled = true; 
            _anchorDetected = true;
        }
    }

    private void InitializeComponents() {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
    }

    private void SetupEventListeners() {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args) {
        DetachFromAnchor();
    }

    private void DetachFromAnchor() {
        if (transform.parent == _anchorTransform) transform.parent = null;
    }

    private void HandleAnchorStay(Collider other) {
        _anchorTransform = other.transform;
    }

    private void HandleAnchorExit(Collider other) {
        _anchorDetected = false;
        other.GetComponent<CableAnchor>().DeactivateGhost();
    }

    private void OnRelease(SelectExitEventArgs args) {
        if (_anchorDetected) HandleReleaseNearAnchor();
    }

    private void HandleReleaseNearAnchor() {
        SnapToAnchor();
    }

    private void SnapToAnchor() {
        ResetRigidbodyVelocities();
        AttachToAnchor();
        GhostMeshRenderer.enabled = false;
    }

    private void ResetRigidbodyVelocities() {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void AttachToAnchor() {
        transform.position = _anchorTransform.position;
        transform.rotation = _anchorTransform.rotation;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.isKinematic = true;
        FusibleOk = true;
    }
}