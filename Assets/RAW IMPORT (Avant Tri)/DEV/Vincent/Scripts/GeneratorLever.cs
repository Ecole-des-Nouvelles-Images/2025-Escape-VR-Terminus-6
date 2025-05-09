using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GeneratorLever : MonoBehaviour
{
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

    private Quaternion _targetRotation;
    private float _interpolationDuration = 1.5f; // Durée de l'interpolation en secondes
    private float _interpolationTimer;

    private void Awake()
    {
        InitializeComponents();
        SetupEventListeners();
        InitializeValues();
    }

    private void Update()
    {
        _leverRotation = transform.localRotation.eulerAngles.z;
        if (!_isMovable)
        {
            // Incrémente le timer d'interpolation
            _interpolationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_interpolationTimer / _interpolationDuration);

            // Interpolation vers la rotation cible
            transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, t);

            // Vérifiez si la rotation est proche de la cible pour mettre à jour LeverActivated
            if (t >= 1.0f)
            {
                if (_targetRotation == Quaternion.Euler(0, 0, _minRot))
                {
                    LeverActivated = true;
                }
                else if (_targetRotation == Quaternion.Euler(0, 0, _maxRot))
                {
                    LeverActivated = false;
                }
                _interpolationTimer = 0; // Réinitialise le timer pour la prochaine interpolation
            }
        }
    }

    private void InitializeComponents()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void InitializeValues()
    {
        _minRot = 250;
        _maxRot = 360;
        _mid = 270;
    }

    private void SetupEventListeners()
    {
        _grabInteractable.selectEntered.AddListener(OnGrab);
        _grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        UnlockLever();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        LockLever();
    }

    private void UnlockLever()
    {
        _isMovable = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void LockLever()
    {
        _isMovable = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        _interpolationTimer = 0; // Réinitialise le timer pour la prochaine interpolation

        if (_leverRotation >= 200 && _leverRotation < 280)
        {
            _targetRotation = Quaternion.Euler(0, 0, _minRot);
        }
        else
        {
            _targetRotation = Quaternion.Euler(0, 0, _maxRot);
        }
    }
}
