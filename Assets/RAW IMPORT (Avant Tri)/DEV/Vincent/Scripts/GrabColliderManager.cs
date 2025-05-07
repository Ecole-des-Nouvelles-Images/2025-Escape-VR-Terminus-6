using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabColliderManager : MonoBehaviour
{
    public Collider colliderRight;
    public Collider colliderLeft;
    [SerializeField] private XRBaseInteractor _interactor;

    private void Awake()
    {
        _interactor = GetComponentInChildren<XRBaseInteractor>();
        if (_interactor != null)
        {
            _interactor.selectEntered.AddListener(OnSelectEnter);
            _interactor.selectExited.AddListener(OnSelectExit);
        }
        else
        {
            Debug.LogError("No XRBaseInteractor component found on the GameObject.");
        }
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (colliderRight != null)
        {
            colliderRight.enabled = false;
        }
        if (colliderLeft != null)
        {
            colliderLeft.enabled = false;
        }
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        if (colliderRight != null)
        {
            colliderRight.enabled = true;
        }
        if (colliderLeft != null)
        {
            colliderLeft.enabled = true;
        }
    }
}