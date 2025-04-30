using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class AnchorManager : MonoBehaviour
{
    public Transform AnchorTransform; // L'ancre pour "snap" l'objet
    public float AnchorRange = 0.1f; // Portée de l'ancre
    public MeshRenderer SnapIndicator; // Indicateur visuel pour le snap

    private Rigidbody _objectRigidbody;
    private bool _isOnBase = true;

    private void Start() {
        InitializeComponents();
        SetupAnchorCollider();
        Debug.Log("AnchorManager initialized. Anchor range: " + AnchorRange);
    }
    private void InitializeComponents() {
        _objectRigidbody = GetComponent<Rigidbody>();
    }
    private void SetupAnchorCollider() {
        SphereCollider anchorCollider = AnchorTransform.gameObject.AddComponent<SphereCollider>();
        anchorCollider.isTrigger = true;
        anchorCollider.radius = AnchorRange;
    }

    public void OnGrab(SelectEnterEventArgs args) {
        Debug.Log("OnGrab called");
        DetachFromAnchor();
        EnableGravityAndConstraints(true, RigidbodyConstraints.None);
        _isOnBase = false;
    }
    public void OnRelease(SelectExitEventArgs args) {
        Debug.Log("OnRelease called");
        if (IsWithinAnchorRange()) { SnapToAnchor(); }
        else { EnableGravity(true); }
    }
    private bool IsWithinAnchorRange() {
        return Vector3.Distance(transform.position, AnchorTransform.position) < AnchorRange;
    }

    public void SnapToAnchor() {
        Debug.Log("SnapToAnchor called");
        _isOnBase = true;
        EnableGravityAndConstraints(false, RigidbodyConstraints.FreezeAll);
        ResetVelocities();
        MatchAnchorTransform();
        AttachToAnchor();
    }
    private void DetachFromAnchor() {
        if (transform.parent == AnchorTransform) {
            transform.parent = null;
        }
    }

    private void EnableGravityAndConstraints(bool useGravity, RigidbodyConstraints constraints) {
        _objectRigidbody.useGravity = useGravity;
        _objectRigidbody.constraints = constraints;
    }

    private void EnableGravity(bool useGravity) {
        _objectRigidbody.useGravity = useGravity;
    }

    private void ResetVelocities() {
        _objectRigidbody.velocity = Vector3.zero;
        _objectRigidbody.angularVelocity = Vector3.zero;
    }

    private void MatchAnchorTransform() {
        transform.position = AnchorTransform.position;
        transform.rotation = AnchorTransform.rotation;
    }

    private void AttachToAnchor() {
        transform.parent = AnchorTransform;
    }

    public void UpdateAnchorIndicator() {
        SnapIndicator.enabled = IsWithinAnchorRange() && !_isOnBase;
    }
}
