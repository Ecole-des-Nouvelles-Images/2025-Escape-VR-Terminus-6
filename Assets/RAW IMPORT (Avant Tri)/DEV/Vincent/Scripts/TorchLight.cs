using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TorchLight : MonoBehaviour
{
    [Header("Light Components")]
    public Light lampLight;
    public MeshRenderer lampMeshRenderer;
    public MeshRenderer lightRayMesh;

    [Header("Materials")]
    public Material lampMaterialOn;
    public Material lampMaterialOff;

    [Header("Input Actions")]
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    private XRGrabInteractable _grabInteractable;
    private IXRSelectInteractor _currentInteractor;
    private bool _isLampOn;

    private void Awake() {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabInteractable.selectEntered.AddListener(OnSelectEnter);
        _grabInteractable.selectExited.AddListener(OnSelectExit);
        ToggleLamp();
    }
    private void OnSelectEnter(SelectEnterEventArgs args) {
        _currentInteractor = args.interactorObject;
    }
    private void OnSelectExit(SelectExitEventArgs args) {
        _currentInteractor = null;
    }
    private void Update() {
        if (_grabInteractable.isSelected && _currentInteractor != null) {
            if ((_currentInteractor.transform.CompareTag("LeftHand") && leftTriggerAction.action.triggered) ||
                (_currentInteractor.transform.CompareTag("RightHand") && rightTriggerAction.action.triggered)) {
                ToggleLamp();
            }
        }
    }
    private void ToggleLamp() {
        _isLampOn = !_isLampOn;
        lampLight.enabled = _isLampOn;
        lampMeshRenderer.material = _isLampOn ? lampMaterialOn : lampMaterialOff;
        lightRayMesh.enabled = _isLampOn;
    }
}