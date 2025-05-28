using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayerFsm : MonoBehaviour
{
    [SerializeField] private Transform cdAnchor;
    [SerializeField] private MeshRenderer cdGhost;
    [SerializeField] private Enigma enigma;
    [SerializeField] private GameObject screenLight;

    public Transform transformStartPosition;
    public Transform transformInPosition;
    public Transform transformOutPosition;

    public float startInTimer;
    public float inOutTimer;
    public float resetTimer;

    private bool isCdIn;
    private bool hasSolvedEnigma;
    private VideoPlayer videoPlayer;
    private BoxCollider boxCollider;
    private GameObject currentCd;

    public enum LectorStates
    {
        OutStatic,
        In,
        InStatic,
        Out
    }

    public LectorStates LectorState = LectorStates.OutStatic;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        boxCollider = GetComponent<BoxCollider>();
        cdGhost.enabled = false;
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
            currentCd = other.gameObject;
            LectorState = LectorStates.In;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("CD")) return;
        cdGhost.enabled = false;
    }

    private void Update()
    {
        switch (LectorState)
        {
            case LectorStates.OutStatic:
                ManageOutStatic();
                break;
            case LectorStates.In:
                ManageIn();
                break;
            case LectorStates.InStatic:
                ManageInStatic();
                break;
            case LectorStates.Out:
                ManageOut();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ManageOutStatic()
    {
        // Rester en position "transformStartPosition" et attendre d'être inséré
        transform.position = transformStartPosition.position;
    }

    private void ManageIn()
    {
        isCdIn = true;
        Rigidbody cdRigidbody = currentCd.GetComponent<Rigidbody>();
        Collider cdCollider = currentCd.GetComponent<Collider>();

        cdRigidbody.velocity = Vector3.zero;
        cdRigidbody.isKinematic = true;
        cdRigidbody.useGravity = false;
        cdCollider.enabled = false;

        cdGhost.enabled = false;
        currentCd.transform.SetParent(cdAnchor, false);
        currentCd.transform.localPosition = Vector3.zero;
        currentCd.transform.localRotation = Quaternion.identity;
        currentCd.transform.localScale = Vector3.one;

        boxCollider.enabled = false;

        StartCoroutine(MoveCdToPosition(transformInPosition.position, startInTimer));
    }

    private void ManageInStatic()
    {
        if (videoPlayer.time >= videoPlayer.clip.length - 0.25f)
        {
            if (!hasSolvedEnigma)
            {
                enigma.Solve.Invoke();
                hasSolvedEnigma = true;
            }
            LectorState = LectorStates.Out;
        }
    }

    private void ManageOut()
    {
        StartCoroutine(MoveCdToPosition(transformOutPosition.position, inOutTimer));
    }

    private IEnumerator MoveCdToPosition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (LectorState == LectorStates.Out)
        {
            ResetToOutStatic();
        }
        else if (LectorState == LectorStates.In)
        {
            LectorState = LectorStates.InStatic;
            videoPlayer.Play();
        }
    }

    private void ResetToOutStatic()
    {
        Rigidbody cdRigidbody = currentCd.GetComponent<Rigidbody>();
        Collider cdCollider = currentCd.GetComponent<Collider>();

        cdRigidbody.velocity = Vector3.zero;
        cdRigidbody.isKinematic = false;
        cdRigidbody.useGravity = true;
        cdCollider.enabled = true;
        currentCd.transform.SetParent(null);

        StartCoroutine(ResetOutTimer());
    }

    private IEnumerator ResetOutTimer()
    {
        yield return new WaitForSeconds(resetTimer);
        boxCollider.enabled = true;
        LectorState = LectorStates.OutStatic;
        isCdIn = false;
    }
}
