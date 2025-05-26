using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Door : MonoBehaviour
{
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private float _doorRotationValue;
    [SerializeField] private float _sensitivity = 1.0f; 

    void Update() {
        _doorRotationValue = transform.rotation.eulerAngles.y;
        if (_doorRotationValue > _hingeJoint.limits.max) { _doorRotationValue -= 360; }
        this.transform.rotation = Quaternion.Euler(new Vector3(0, _doorRotationValue, 0));
    }
}