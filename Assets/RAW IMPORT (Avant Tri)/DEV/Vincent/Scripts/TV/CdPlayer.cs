using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayer : MonoBehaviour {
    [SerializeField] private Transform _anchor;
    [SerializeField] private MeshRenderer _cdGhost;
    [SerializeField] private Enigma _enigma;
    private GameObject _currentCd;
    private bool _isCdIn;
    private float _playbackTime;

    private BoxCollider _boxCollider;
    private VideoPlayer _videoPlayer;

    private void Awake() {
        _videoPlayer = GetComponent<VideoPlayer>();
        _cdGhost.enabled = false;
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {
        _playbackTime = (float)_videoPlayer.time;
        if (_playbackTime >= _videoPlayer.clip.length - .5f && _isCdIn) {
            _isCdIn = false;
            _enigma.Solve.Invoke();
            if (_currentCd != null)
            {
                EjectCd();
            }
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
            _boxCollider.enabled = false;
            _currentCd.GetComponent<Collider>().enabled = false;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
            rb.AddForce(new Vector3(0,0,-1f) * 0.5f, ForceMode.Impulse);
            StartCoroutine(EjectCdMiniTimer());
        }
    }

    private IEnumerator EjectCdMiniTimer()
    {
        var rb = _currentCd.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        yield return new WaitForSeconds(1f);
        _boxCollider.enabled = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        GetComponent<Animator>().SetTrigger("Reset");
        _currentCd.GetComponent<Collider>().enabled = true;
        _currentCd.transform.parent = null;
        _currentCd = null;
        _isCdIn = false;
        
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
        cd.transform.localScale = Vector3.one;

        _currentCd = cd;
        _isCdIn = true;

        GetComponent<Animator>().SetTrigger("TriggerCdIn");
        yield return new WaitForSeconds(0.33f);

        _videoPlayer.Play();
    }
}