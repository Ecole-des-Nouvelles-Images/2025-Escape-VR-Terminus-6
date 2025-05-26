using System.Collections.Generic;
using UnityEngine;

public class InteractableReset : MonoBehaviour {
    [SerializeField] private Dictionary<GameObject, (Vector3, Quaternion)> _originalTransforms = new();

    private void Start() {
        var interactables = GameObject.FindGameObjectsWithTag("Interactable");
        var cds = GameObject.FindGameObjectsWithTag("CD");
        var allObjects = new GameObject[interactables.Length + cds.Length];
        interactables.CopyTo(allObjects, 0);
        cds.CopyTo(allObjects, interactables.Length);
        foreach (var obj in allObjects)
            if (!_originalTransforms.ContainsKey(obj))
                _originalTransforms[obj] = (obj.transform.position, obj.transform.rotation);
    }

    private void OnTriggerEnter(Collider other) {
        if ((!other.CompareTag("Interactable") && !other.CompareTag("CD")) ||
            !_originalTransforms.ContainsKey(other.gameObject)) return;
        Debug.Log(other.gameObject.name + " Position Reset");
        other.transform.position = _originalTransforms[other.gameObject].Item1;
        other.transform.rotation = _originalTransforms[other.gameObject].Item2;
        var rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}