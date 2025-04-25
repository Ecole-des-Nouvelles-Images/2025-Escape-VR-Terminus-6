using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Lever : MonoBehaviour {
    public float leverValue;
    [Header("Components")]
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private XRGrabInteractable _xrGrabInteractable;
    
    [Header("Values")]
    [SerializeField] private float _leverMargin;
    [SerializeField] private float _maxInteractionDistance;
    
    private float _leverRotation, _trueLeverRotation;
    private float _min, _max, _mid;
    private float _lomid, _himid;
    private bool _isMovable;

    public void Unlock() {
        _isMovable = true;
        Debug.Log("Lever Unlocked");
    }

    public void Lock() {
        Debug.Log("Lever Locked");
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