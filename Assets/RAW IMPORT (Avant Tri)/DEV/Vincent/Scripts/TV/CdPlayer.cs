using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayer : MonoBehaviour {
    [SerializeField] private Transform _anchor;
    [SerializeField] private MeshRenderer _cdGhost;
    private GameObject _currentCd;
    private bool _isCdIn;

    private VideoPlayer _videoPlayer;

    private void Awake() {
        _videoPlayer = GetComponent<VideoPlayer>();
        _cdGhost.enabled = false;
    }

    private void Update() {
        if (_videoPlayer.time >= _videoPlayer.clip.length && _isCdIn) {
            _isCdIn = false;
            EjectCd();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!other.CompareTag("CD")) return;

        var grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable.isSelected)
            _cdGhost.enabled = true;
        else
            StartCoroutine(InsertCd(other.gameObject));
    }

    private void EjectCd() {
        if (_currentCd != null) {
            var rb = _currentCd.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * 15, ForceMode.Impulse);
        }
    }

    private IEnumerator InsertCd(GameObject cd) {
        var rb = cd.GetComponent<Rigidbody>();
        var collider = cd.GetComponent<Collider>();

        collider.enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        _cdGhost.enabled = false;
        cd.transform.SetParent(_cdGhost.transform, false);
        cd.transform.localPosition = Vector3.zero;
        cd.transform.localRotation = Quaternion.identity;

        _currentCd = cd;
        _isCdIn = true;

        GetComponent<Animator>().SetTrigger("TriggerCdIn");
        yield return new WaitForSeconds(0.33f);

        _videoPlayer.Play();
    }
}