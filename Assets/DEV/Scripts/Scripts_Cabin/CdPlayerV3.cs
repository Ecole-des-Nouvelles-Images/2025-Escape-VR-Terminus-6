using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayerV3 : MonoBehaviour
{
    [SerializeField] private Transform cdAnchor;
    [SerializeField] private MeshRenderer cdGhost;
    [SerializeField] private Enigma enigma;
    [SerializeField] private GameObject light;

    private GameObject currentCd;
    private bool isCdIn;
    private bool hasSolvedEnigma;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private Animator animator;

    private bool inserting;
    private bool ejecting;
    
    private void Awake()
    {
        light.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        cdGhost.enabled = false;
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("CD")) return;
        var grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable.isSelected)
        {
            cdGhost.enabled = true;
        }
        else if (!isCdIn)
        {
            StartInsertCd(other.gameObject);
        }
    }

    private void StartInsertCd(GameObject cd)
    {
        if (isCdIn) return;
        if (cd == null) return;
        
        isCdIn = true;
        Rigidbody cdRigidbody = cd.GetComponent<Rigidbody>();
        Collider cdCollider = cd.GetComponent<Collider>();
        
        cdRigidbody.velocity = Vector3.zero;
        cdRigidbody.isKinematic = true;
        cdRigidbody.useGravity = false;
        cdCollider.enabled = false;
        
        cdGhost.enabled = false;
        cd.transform.SetParent(cdAnchor, false);
        cd.transform.localPosition = Vector3.zero;
        cd.transform.localRotation = Quaternion.identity;
        cd.transform.localScale = Vector3.one;
        
        currentCd = cd;
        boxCollider.enabled = false;
        animator.SetTrigger("InsertCd");
        animator.SetBool("Inserting", true);
        videoPlayer.Play();
    }

    private void Update()
    {
        if (videoPlayer.time >= videoPlayer.clip.length - 0.25f)
        {
            if (isCdIn && !hasSolvedEnigma && !ejecting)
            {
                
                animator.SetBool("Inserting", false);
                enigma.Solve.Invoke();
                hasSolvedEnigma = true;
                StartCoroutine(EjectCd(currentCd));
                light.SetActive(false);
            }
            else if (isCdIn && hasSolvedEnigma && !ejecting)
            {
                animator.SetBool("Inserting", false);
                StartCoroutine(EjectCd(currentCd));
                light.SetActive(false);
            }
        }
    }

    private IEnumerator EjectCd(GameObject cd)
    {
        Debug.Log("Ejecting CD");
        if (cd == null) yield break;
        if (ejecting) yield break;
        ejecting = true;
        
        animator.SetBool("Ejecting", true);
        Rigidbody cdRigidbody = cd.GetComponent<Rigidbody>();
        Collider cdCollider = cd.GetComponent<Collider>();
        
        yield return new WaitForSeconds(0.9f);
        
        animator.SetBool("Ejecting", false);
        cdRigidbody.velocity = Vector3.zero;
        cdRigidbody.isKinematic = false;
        cdRigidbody.useGravity = true;
        cdCollider.enabled = true;
        
        cdGhost.enabled = true;
        cd.transform.SetParent(null);
        

        yield return new WaitForSeconds(0.15f);

        ejecting = false;
        animator.SetTrigger("Reset");
        
        yield return new WaitForSeconds(0.15f);
        
        animator.ResetTrigger("Reset");
        isCdIn = false;
        boxCollider.enabled = true;
        
        yield break;
    }
}
