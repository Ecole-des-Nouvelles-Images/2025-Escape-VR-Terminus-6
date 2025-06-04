using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CableHead : MonoBehaviour {
    public LineRenderer LineRenderer;
    public Transform CableBase;
    public Material ConnectedMaterial;
    public UnityEvent Connected;
    
    [SerializeField] private bool _isConnected;
    private bool _anchorDetected;
    private Transform _anchorTransform;
    private XRGrabInteractable _grabInteractable;
    public Transform baseAnchorPos;
    private Vector3 _originalPosition;
    private Rigidbody _rb;
    public CableAnchor tempCableAnchor;
    private bool _isReturning = false; // Nouveau booléen pour le retour progressif
    public bool CableHeadUnlocked;
    private void Start() {
        InitializeComponents();
        SetupEventListeners();
        _originalPosition = baseAnchorPos.transform.position;
        LockHead();
    }

    public void LockHead() {
        CableHeadUnlocked = false;
        if (_grabInteractable != null) {
            _grabInteractable.enabled = CableHeadUnlocked;
        }
    }

    public void UnlockHead() {
        CableHeadUnlocked = true;
        _grabInteractable.enabled = CableHeadUnlocked;
    }

    private void Update() {
        UpdateLineRenderer();
        if (_isReturning) {
            ReturnToBase();
        }
    }

    private void FixedUpdate() {
        ResetRotation();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Anchor")) HandleAnchorExit(other);
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Anchor")) HandleAnchorStay(other);
    }

    private void InitializeComponents() {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>();
    }

    private void SetupEventListeners() {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void UpdateLineRenderer() {
        LineRenderer.SetPosition(0, CableBase.position);
        LineRenderer.SetPosition(1, transform.position);
        LineRenderer.material = ConnectedMaterial;
    }

    private void ResetRotation() {
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private void OnGrab(SelectEnterEventArgs args) {
        DetachFromAnchor();
        tempCableAnchor?.ColorOff();
        tempCableAnchor?.SetCablePlugged(false);
        _isReturning = false; // Réinitialiser le retour lors de la saisie
    }

    public void DetachFromAnchor() {
        if (transform.parent == _anchorTransform) transform.parent = null;
        _isConnected = false;
    }

    private void HandleAnchorStay(Collider other) {
        tempCableAnchor = other.GetComponent<CableAnchor>();
        _anchorTransform = tempCableAnchor.Ghost.transform;
        _anchorDetected = true;
        tempCableAnchor.ActivateGhost();
    }

    private void HandleAnchorExit(Collider other) {
        _anchorDetected = false;
        other.GetComponent<CableAnchor>().DeactivateGhost();
    }

    private void OnRelease(SelectExitEventArgs args) {
        if (!_isConnected) {
            _isReturning = true;
        }
        if (_anchorDetected) HandleReleaseNearAnchor();
    }

    private void HandleReleaseNearAnchor() {
        if (tempCableAnchor.CablePlugged) {
            ReturnToBase();
            tempCableAnchor.CablePlugged = true;
        }
        else {
            SnapToAnchor();
        }
    }

    private void SnapToAnchor() {
        Connected.Invoke();
        tempCableAnchor.SetCablePlugged(true);
        ResetRigidbodyVelocities();
        AttachToAnchor();
        if (tempCableAnchor.CableOk) {
            GetComponent<Collider>().enabled = false;
            tempCableAnchor.GetComponent<Collider>().enabled = false;
        }
    }

    private void ResetRigidbodyVelocities() {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void AttachToAnchor() {
        transform.position = _anchorTransform.position;
        _isConnected = true;
    }

    public void ReturnToBase() {
        // Interpolation vers la position de base
        if (_isConnected) return; 
        transform.position = Vector3.Lerp(transform.position, _originalPosition, .3f);

        // Vérifier si la position est proche de la position de base pour arrêter le retour
        if (Vector3.Distance(transform.position, _originalPosition) < 0.01f) {
            _isReturning = false;
        }
    }
}
