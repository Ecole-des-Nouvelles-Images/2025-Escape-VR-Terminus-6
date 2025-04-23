using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DistanceBasedRelease : MonoBehaviour 
{
    public XRGrabInteractable grabInteractable;
    public float maxDistance = 2.0f;
    public Vector3 originalPosition;

    private const float ReleaseDelay = 0.5f;

    private void Start() { InitializeGrabInteractable(); }
    private void InitializeGrabInteractable() {
        if (grabInteractable == null) {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }
    }

    private void Update() {
        if (grabInteractable.isSelected) {
            CheckDistanceAndReleaseIfNeeded();
        }
    }

    private void CheckDistanceAndReleaseIfNeeded() {
        float distance = Vector3.Distance(transform.position, originalPosition);
        if (distance > maxDistance) {
            StartCoroutine(Release());
        }
    }

    private void OnDrawGizmosSelected() {
        DrawDistanceGizmos();
    }
    private void DrawDistanceGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(originalPosition, maxDistance);
        Gizmos.DrawLine(originalPosition, originalPosition + Vector3.up * maxDistance);
    }

    private IEnumerator Release() {
        DisableGrabInteractable();
        yield return new WaitForSeconds(ReleaseDelay);
        EnableGrabInteractable();
    }

    private void DisableGrabInteractable() {
        grabInteractable.enabled = false;
    }
    private void EnableGrabInteractable() {
        grabInteractable.enabled = true;
    }
}