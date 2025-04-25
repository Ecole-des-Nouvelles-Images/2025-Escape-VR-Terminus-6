using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class CableHead : MonoBehaviour {
    public LineRenderer LineRenderer;
    public Transform CableBase;
    public Material ConnectedMaterial;

    [SerializeField] private bool _isConnected = false;
    private XRGrabInteractable _grabInteractable;
    private Rigidbody _rb;
    private Transform _anchorTransform;
    private bool _anchorDetected;
    private CableAnchor _tempCableAnchor;
    private Vector3 _originalPosition;

    private void Start() {
        InitializeComponents();
        SetupEventListeners();
        _originalPosition = transform.position;
    }
    private void InitializeComponents() {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
    }
    private void SetupEventListeners() {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }
    private void Update() { UpdateLineRenderer(); }
    private void UpdateLineRenderer() {
        LineRenderer.SetPosition(0, CableBase.position);
        LineRenderer.SetPosition(1, transform.position);
        LineRenderer.material = ConnectedMaterial;
    }
    private void FixedUpdate() { ResetRotation(); }
    private void ResetRotation() { transform.rotation = Quaternion.Euler(Vector3.zero); }
    private void OnGrab(SelectEnterEventArgs args) {
        DetachFromAnchor();
        _tempCableAnchor?.ColorOff();
        _tempCableAnchor?.SetCablePlugged(false);
    }
    private void DetachFromAnchor() { if (transform.parent == _anchorTransform) { transform.parent = null; } }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Anchor")) { HandleAnchorStay(other); }
    }
    private void HandleAnchorStay(Collider other) {
        _tempCableAnchor = other.GetComponent<CableAnchor>();
        _anchorTransform = other.transform;
        _anchorDetected = true;
        _tempCableAnchor.ActivateGhost();
    }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Anchor")) { HandleAnchorExit(other); } }
    private void HandleAnchorExit(Collider other) {
        _anchorDetected = false;
        other.GetComponent<CableAnchor>().DeactivateGhost();
    }
    private void OnRelease(SelectExitEventArgs args) { if (_anchorDetected) { HandleReleaseNearAnchor(); } }
    private void HandleReleaseNearAnchor() {
        if (_tempCableAnchor.CablePlugged) { transform.position = _originalPosition; }
        else { SnapToAnchor(); }
    }
    private void SnapToAnchor() {
        _tempCableAnchor.VerifyColor(ConnectedMaterial);
        _tempCableAnchor.SetCablePlugged(true);
        ResetRigidbodyVelocities();
        AttachToAnchor();
    }
    private void ResetRigidbodyVelocities() {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
    private void AttachToAnchor() {
        transform.position = _anchorTransform.position;
        transform.parent = _anchorTransform;
    }
}
