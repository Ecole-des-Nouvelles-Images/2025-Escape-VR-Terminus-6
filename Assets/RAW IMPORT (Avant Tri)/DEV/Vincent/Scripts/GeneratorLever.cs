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
        _leverRotation = transform.localRotation.eulerAngles.z;
        Debug.Log(transform.localRotation.eulerAngles.z);
        if (!_isMovable) return;
        //transform.localRotation = Quaternion.Euler( 0, 0,_leverRotation);
    }
    private void InitializeComponents() {
        _hingeJoint = GetComponent<HingeJoint>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void InitializeValues() {
        _minRot = 250;
        _maxRot = 360;
        _mid = 270;
    }
    private void SetupEventListeners() {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }
    private void OnGrab(SelectEnterEventArgs args) { UnlockLever(); }
    private void OnRelease(SelectExitEventArgs args) { LockLever(); }
    private void UnlockLever() { _isMovable = true; GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; }
    private void LockLever() {
        _isMovable = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (_leverRotation is >= 200 and < 280) {
            transform.localRotation = Quaternion.Euler(0, 0,_minRot );
            LeverActivated = true;
        }
        else {
            transform.localRotation = Quaternion.Euler( 0,0 ,_maxRot);
            LeverActivated = false;
        }
    }
}