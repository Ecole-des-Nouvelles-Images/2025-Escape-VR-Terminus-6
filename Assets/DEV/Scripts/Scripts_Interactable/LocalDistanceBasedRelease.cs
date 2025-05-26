using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LocalDistanceBasedRelease : MonoBehaviour
{
    public XRGrabInteractable GrabInteractable;
    public float MaxDistance = 2.0f;
    private const float ReleaseDelay = 0.5f;

    private void Start() { InitializeGrabInteractable(); }
    private void InitializeGrabInteractable() {
        if (GrabInteractable == null) {
            GrabInteractable = GetComponent<XRGrabInteractable>();
        }
    }

    private void Update() {
        if (GrabInteractable.isSelected) {
            CheckDistanceAndReleaseIfNeeded();
        }
    }

    private void CheckDistanceAndReleaseIfNeeded() {
        IXRSelectInteractor interactor = GrabInteractable.firstInteractorSelecting;
        if (interactor != null) {
            Vector3 handPosition = interactor.transform.position;
            float distance = Vector3.Distance(handPosition, transform.position);

            if (distance > MaxDistance) {
                StartCoroutine(Release());
            }
        }
    }

    private void OnDrawGizmosSelected() {
        DrawDistanceGizmos();
    }
    private void DrawDistanceGizmos() { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MaxDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * MaxDistance);
    }

    private IEnumerator Release() {
        DisableGrabInteractable();
        yield return new WaitForSeconds(ReleaseDelay);
        EnableGrabInteractable();
    }

    private void DisableGrabInteractable() {
        GrabInteractable.enabled = false;
    }
    private void EnableGrabInteractable() {
        GrabInteractable.enabled = true;
    }
}