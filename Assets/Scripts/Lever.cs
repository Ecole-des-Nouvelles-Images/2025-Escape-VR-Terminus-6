using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Lever : MonoBehaviour {
    public float leverValue;
    [Header("Components")]
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private TMP_Text _leverValueText;
    [SerializeField] private XRGrabInteractable _xrGrabInteractable;
    
    [Header("Values")]
    [SerializeField] private float _leverMargin;
    [SerializeField] private float _maxInteractionDistance;
    
    private float _leverRotation, _trueLeverRotation;
    private float _min, _max, _positivemid, _negativemid;

    private void Start() {
        _min = _hingeJoint.limits.min;
        _max = _hingeJoint.limits.max;
        _positivemid = _hingeJoint.limits.max / 2;
        _negativemid = _hingeJoint.limits.min / 2;
    }

    void Update() {
        _leverRotation = transform.rotation.eulerAngles.x;
        _trueLeverRotation = _leverRotation;
        
        if (_leverRotation > 180)
            _trueLeverRotation -= 360;

        if (_trueLeverRotation > _positivemid) 
            leverValue = 0.5f;
        else if ((_trueLeverRotation + _leverMargin) >= _max)
            leverValue = 1f;
        else
            leverValue = 0f;
        
        /*_leverRotationClamped = Mathf.Clamp(_leverRotationValue, _hingeJoint.limits.min, _hingeJoint.limits.max);
        leverNorm = _leverRotationClamped / _hingeJoint.limits.max;*/
        
        _leverValueText.text = leverValue.ToString("#0.000");
        this.transform.rotation = Quaternion.Euler(new Vector3(_trueLeverRotation, 0, 0));
    }
}