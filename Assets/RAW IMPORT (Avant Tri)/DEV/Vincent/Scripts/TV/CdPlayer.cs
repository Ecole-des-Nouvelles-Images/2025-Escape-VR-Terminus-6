using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayer : MonoBehaviour
{
    public Transform Anchor;
    private VideoPlayer _videoPlayer;
    public MeshRenderer CdGhost;
    private GameObject _cd;
    private bool _cdIn;

    private void Awake() {
        _videoPlayer = GetComponent<VideoPlayer>();
        CdGhost.enabled = false;
    }

    private void OnTriggerStay(Collider other) {
        if (!other.CompareTag("CD")) return;
        if (other.GetComponent<XRGrabInteractable>().isSelected) { CdGhost.enabled = true; }
        else {
            StartCoroutine(CDIn(other.gameObject));
            //other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void Update() {
        if (_videoPlayer.time >= _videoPlayer.clip.length && _cdIn) { _cdIn = false; EjectCd(); }
    }
    private void EjectCd() { _cd.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,15), ForceMode.Impulse); }

    private IEnumerator CDIn(GameObject cd) {
        cd.GetComponent<Collider>().enabled = false;
        cd.GetComponent<Rigidbody>().useGravity = false;
        cd.GetComponent<Rigidbody>().isKinematic = false;
        cd.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        CdGhost.enabled = false;
        cd.transform.parent = CdGhost.transform;
        cd.transform.position = CdGhost.transform.position;
        cd.transform.rotation = CdGhost.transform.rotation;
        _cd = cd;
        _cdIn = true;
        GetComponent<Animator>().SetTrigger("TriggerCdIn");
        yield return new WaitForSeconds(0.33f);
        _videoPlayer.Play();
    }
}
