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
            CdGhost.enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<MeshRenderer>().enabled = false;
            _videoPlayer.Play();
            _cd = other.gameObject;
            _cd.transform.position = Anchor.position;
            _cdIn = true;
        }
    }
    private void Update() {
        if (_videoPlayer.time >= _videoPlayer.clip.length && _cdIn) { _cdIn = false; EjectCd(); }
    }
    private void EjectCd() { _cd.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,15), ForceMode.Impulse); }
}
