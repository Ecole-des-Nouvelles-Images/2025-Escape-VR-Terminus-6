using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GeneratorLever : MonoBehaviour {
    [Header("General Values")]
    public bool LeverActivated;
    [SerializeField] private bool _isMovable;
    [Header("Components")]
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private XRGrabInteractable _grabInteractable;
    [Header("Rotations Values")]
    [SerializeField] private float _leverRotation;
    [SerializeField] private float _mid;
    [SerializeField] private float _minRot, _maxRot;

    private void Awake() {
        InitializeComponents();
        SetupEventListeners();
        InitializeValues();
    }
    private void Update() {
        if (!_isMovable) return;
        _leverRotation = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(0, 0, _leverRotation);
    }
    private void InitializeComponents() {
        _hingeJoint = GetComponent<HingeJoint>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void InitializeValues() {
        _minRot = _hingeJoint.limits.max;
        _maxRot = _hingeJoint.limits.min;
        _mid = _hingeJoint.limits.min / 2;
    }
    private void SetupEventListeners() {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }
    private void OnGrab(SelectEnterEventArgs args) { UnlockLever(); }
    private void OnRelease(SelectExitEventArgs args) { LockLever(); }
    private void UnlockLever() { _isMovable = true; }
    private void LockLever() {
        _isMovable = false;
        if (_leverRotation < _mid) {
            transform.localRotation = Quaternion.Euler(0, 0, _minRot);
            LeverActivated = false;
        }
        else if (_leverRotation > _mid) {
            transform.localRotation = Quaternion.Euler(0, 0, _maxRot);
            LeverActivated = true;
        }
    }
}