using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour {
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    private Vector3 _baseScale;
    private Vector3 _colliderScale;
    private Material _material;

    public Color EmissionColor = Color.black;
    public Color PressedEmissionColor = Color.white;
    public Color CooldownColor = Color.red;

    private int _currentMaterialId;

    private bool _isPressed = false;
    private const float PressedScaleFactor = 0.75f;
    private const float ColliderScaleFactor = 1.15f;
    private const float CooldownDuration = 1f;

    private void Awake() { InitializeComponents(); }

    private void InitializeComponents() {
        _baseScale = transform.localScale;
        _colliderScale = GetComponent<BoxCollider>().size;
        _material = GetComponent<Renderer>().material;
        SetMaterialColor(EmissionColor);
    }

    private void OnTriggerStay(Collider other) {
        if (!_isPressed && other.CompareTag("Hand")) { HandleButtonPress(); }
    }

    private void OnTriggerExit(Collider other) {
        if (_isPressed && other.CompareTag("Hand")) { HandleButtonRelease(); }
    }

    private void HandleButtonPress() {
        _isPressed = true;
        SetButtonScale(PressedScaleFactor);
        SetColliderScale(ColliderScaleFactor);
        SetMaterialColor(PressedEmissionColor);
        OnButtonPressed.Invoke();
    }

    private void HandleButtonRelease() {
        ResetButtonScale();
        ResetColliderScale();
        StartCoroutine(ButtonCooldown());
        OnButtonReleased.Invoke();
    }

    private void SetButtonScale(float scaleFactor) {
        transform.localScale = new Vector3(_baseScale.x, _baseScale.y, _baseScale.z * scaleFactor);
    }
    private void ResetButtonScale() {
        transform.localScale = _baseScale;
    }
    private void SetColliderScale(float scaleFactor) {
        GetComponent<BoxCollider>().size = new Vector3(_colliderScale.x, _colliderScale.y, _baseScale.z * scaleFactor);
    }
    private void ResetColliderScale() {
        GetComponent<BoxCollider>().size = _colliderScale;
    }

    private void SetMaterialColor(Color color) {
        _material.color = color;
    }

    private IEnumerator ButtonCooldown() {
        SetMaterialColor(CooldownColor);
        yield return new WaitForSeconds(CooldownDuration);
        SetMaterialColor(EmissionColor);
        _isPressed = false;
    }
}
