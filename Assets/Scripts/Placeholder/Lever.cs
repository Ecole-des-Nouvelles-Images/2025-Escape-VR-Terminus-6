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
    private float _min, _max, _positivemid, _negativemid;
    private bool _isMovable;

    public void Unlock() {
        _isMovable = true;
        Debug.Log("aaaaaaaa");
    }

    /*public void Lock() {
        Debug.Log("bbbbbbbb");
        if (_trueLeverRotation > _positivemid) {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            leverValue = 0f;
        }
        else if ((_trueLeverRotation + _leverMargin) >= _max) {
            transform.rotation = Quaternion.Euler(new Vector3(_max, 0, 0));
            leverValue = 1f;
        }
        else leverValue = 0f;
    }*/
    
    private void Start() {
        _min = _hingeJoint.limits.min;
        _max = _hingeJoint.limits.max;
        _positivemid = _hingeJoint.limits.max / 2;
        _negativemid = _hingeJoint.limits.min / 2;
    }

    void Update() {
        if (_isMovable) {
            Debug.Log("move");
            _leverRotation = transform.rotation.eulerAngles.x;
            _trueLeverRotation = _leverRotation;
            this.transform.rotation = Quaternion.Euler(new Vector3(_trueLeverRotation, 0, 0));
        }
        else if (!_isMovable) {

        }
    }
}