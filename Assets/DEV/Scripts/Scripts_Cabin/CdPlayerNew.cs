using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CdPlayerNew : MonoBehaviour
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
    
    private void Awake()
    {
        light.SetActive(false);
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        cdGhost.enabled = false;
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isCdIn && !hasSolvedEnigma && videoPlayer.time >= videoPlayer.clip.length - 0.5f)
        {
            if (inserting) return;
            enigma.Solve.Invoke();
            hasSolvedEnigma = true;
            StartCoroutine(EjectCd());
            light.SetActive(false);
            isCdIn = false;
        }
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
            StartCoroutine(InsertCd(other.gameObject));
        }
    }

    private IEnumerator InsertCd(GameObject cd)
    {
        inserting = true;
        var rb = cd.GetComponent<Rigidbody>();
        var collider = cd.GetComponent<Collider>();

        rb.velocity = Vector3.zero;
        collider.enabled = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        cdGhost.enabled = false;
        cd.transform.SetParent(cdAnchor, false);
        cd.transform.localPosition = Vector3.zero;
        cd.transform.localRotation = Quaternion.identity;
        cd.transform.localScale = Vector3.one;

        rb.isKinematic = true;
        currentCd = cd;
        isCdIn = true;
        hasSolvedEnigma = false; // Réinitialiser hasSolvedEnigma

        animator.SetTrigger("InsertCd");

        yield return new WaitForSeconds(0.33f);
        videoPlayer.Play();
        light.SetActive(true);
        inserting = false;
    }

    private IEnumerator EjectCd()
    {
        if (currentCd != null)
        {
            animator.SetTrigger("EjectCd");

            // Attendre la fin de l'animation d'éjection
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Désactiver le Box Collider temporairement
            boxCollider.enabled = false;

            var rb = currentCd.GetComponent<Rigidbody>();
            var collider = currentCd.GetComponent<Collider>();

            // Réactiver la physique du CD
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
            collider.enabled = true;
            currentCd.transform.parent = null;
            currentCd = null;

            Debug.Log("Timer Ejected Start");

            // Attendre 2 secondes pour que le CD sorte de la boîte de collision
            yield return new WaitForSeconds(2f);

            Debug.Log("Timer Ejected End");
            // Réactiver la détection après le délai
            boxCollider.enabled = true;

            animator.SetTrigger("Reset");
        }
    }
}
