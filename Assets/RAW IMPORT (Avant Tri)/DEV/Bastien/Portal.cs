using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal destinationPortal;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool isActive = true;
    
    private bool _isHidden;
    private HashSet<Collider> _teleportedObjects = new HashSet<Collider>();

    public event Action<Collider> OnTriggerEntered;
    public event Action<Collider> OnTriggerExited;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
    }
        
    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
    }
    
    private void OnEnable()
    {
        OnTriggerEntered += PortalEntered;
        OnTriggerExited += PortalExited;
    }

    private void OnDisable()
    {
        OnTriggerEntered -= PortalEntered;
        OnTriggerExited -= PortalExited;
    }

    private void PortalEntered(Collider other)
    {
        if (destinationPortal == null || _isHidden) return;
        if (other.CompareTag(playerTag) || other.CompareTag("Teleportable"))
        {
            if (_teleportedObjects.Contains(other)) return;
                
            Transform originTransform = other.GetComponentInParent<XROrigin>().gameObject.transform;
            var offset = originTransform.position - transform.position;
            var offsetRotation = originTransform.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
            originTransform.transform.position = destinationPortal.transform.position + offset;
            //originTransform.transform.eulerAngles = destinationPortal.transform.rotation.eulerAngles + offsetRotation;
            StartCoroutine(MaskOtherPortal());
            _teleportedObjects.Add(other);
                
            if (other.CompareTag(playerTag)) {
                destinationPortal._isHidden = true;
                isActive = false;
                destinationPortal.destinationPortal.isActive = true;
            }
        }
    }
    
    private void PortalExited(Collider other)
    {
        if (destinationPortal == null) return;
            
        if (!other.CompareTag(playerTag)) return;
            
        _isHidden = false;
            
        _teleportedObjects.Remove(other);
    }

    private IEnumerator MaskOtherPortal()
    {
        destinationPortal.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        destinationPortal.GetComponent<MeshRenderer>().enabled = true;
    }
}
