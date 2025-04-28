using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Lever : MonoBehaviour {
    [Header("Essentials")]
    public float leverValue;
    public UnityEvent SpeedChange;
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private XRGrabInteractable _xrGrabInteractable;
    
    [Header("Values")]
    [SerializeField] private float _leverMargin;
    [SerializeField] private float _maxInteractionDistance;
    
    [Header("Debug")]
    private float _leverRotation;   //Self-explanatory
    private float _min, _max, _mid; //Minimum, maximum and middle rotation
    private float _lomid, _himid;   //Values calculated from the middle for padding
    private bool _isMovable;        //Is the lever locked? Not the same as ignoreLever

    public void Unlock() {
        _isMovable = true;
    }

    public void Lock() {
        _isMovable = false;
        if (_leverRotation < _lomid) {
            transform.rotation = Quaternion.Euler(new Vector3(_min, 0f, 0f));
            leverValue = 0f;
        }
        else if (_leverRotation > _lomid && _leverRotation < _himid) {
            transform.rotation = Quaternion.Euler(new Vector3(_mid, 0f, 0f));
            leverValue = 0.5f;
        }
        else if (_leverRotation > _himid || (_leverRotation + _leverMargin) >= _max) {
            transform.rotation = Quaternion.Euler(new Vector3(_max, 0f, 0f));
            leverValue = 1f;
        }
        SpeedChange.Invoke();
    }

    public void Reset() {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        leverValue = 0f;
    }
    
    private void Start() {
        _min = _hingeJoint.limits.min;
        _max = _hingeJoint.limits.max;
        _mid = _hingeJoint.limits.max / 2;
        _lomid = _mid - _leverMargin;
        _himid = _mid + _leverMargin;
    }

    void Update() {
        if (_isMovable) {
            _leverRotation = transform.rotation.eulerAngles.x;
            this.transform.rotation = Quaternion.Euler(new Vector3(_leverRotation, 0, 0));
        }
    }
}