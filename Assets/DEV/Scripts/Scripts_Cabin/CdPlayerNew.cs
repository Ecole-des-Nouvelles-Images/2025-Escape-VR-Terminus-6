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
    private float insertCdTimer;
    private float ejectCdTimer;
    private int ejectCdStep;

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
        if (videoPlayer.time >= videoPlayer.clip.length - 0.25f)
        {
            if (isCdIn && !hasSolvedEnigma)
            {
                enigma.Solve.Invoke();
                hasSolvedEnigma = true;
                StartEjectCd();
                light.SetActive(false);
                isCdIn = false;
            }
            else if (isCdIn && hasSolvedEnigma)
            {
                StartEjectCd();
                light.SetActive(false);
                isCdIn = false;
            }
        }

        if (insertCdTimer > 0)
        {
            insertCdTimer -= Time.deltaTime;
            if (insertCdTimer <= 0)
            {
                FinishInsertCd();
            }
        }

        if (ejectCdTimer > 0)
        {
            ejectCdTimer -= Time.deltaTime;
            if (ejectCdTimer <= 0)
            {
                switch (ejectCdStep)
                {
                    case 1:
                        EnableCollider();
                        break;
                    case 2:
                        ReleaseCd();
                        break;
                    case 3:
                        FinishEjectCd();
                        break;
                    case 4:
                        EnableBoxCollider();
                        break;
                }
                ejectCdStep++;
                if (ejectCdStep <= 4)
                {
                    ejectCdTimer = 1f; // Reset timer for next step
                }
            }
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
            StopAllCoroutines();
            StartInsertCd(other.gameObject);
        }
    }

    private void StartInsertCd(GameObject cd)
    {
        animator.SetBool("Inserting", true);
        animator.SetTrigger("InsertCd");

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
        hasSolvedEnigma = false;
        
        boxCollider.enabled = false;

        insertCdTimer = 0.33f;
    }

    private void FinishInsertCd()
    {
        videoPlayer.Play();
        light.SetActive(true);
        animator.SetBool("Inserting", false);
    }

    private void StartEjectCd()
    {
        if (!isCdIn || inserting)
        {
            return;
        }
        animator.SetBool("Ejecting", true);
        ejectCdTimer = animator.GetCurrentAnimatorStateInfo(0).length;
        ejectCdStep = 1;
    }

    private void EnableCollider()
    {
        var collider = currentCd.GetComponent<Collider>();
        collider.enabled = true;
    }

    private void ReleaseCd()
    {
        var rb = currentCd.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        currentCd.transform.parent = null;
    }

    private void EnableBoxCollider()
    {
        boxCollider.enabled = true;
    }

    private void FinishEjectCd()
    {
        animator.SetBool("Ejecting", false);
        animator.SetTrigger("Reset");
        currentCd = null;
    }
}



/*using System.Collections;
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
        if (videoPlayer.time >= videoPlayer.clip.length - 0.5f)
        {
            if (isCdIn && !hasSolvedEnigma)
            {
                enigma.Solve.Invoke();
                hasSolvedEnigma = true;
                StartCoroutine(EjectCd());
                light.SetActive(false);
                isCdIn = false;
            }
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
            StopAllCoroutines();
            StartCoroutine(InsertCd(other.gameObject));
            
        }
    }

    private IEnumerator InsertCd(GameObject cd)
    {
        animator.SetBool("Inserting", true);
        animator.SetTrigger("InsertCd");

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
        hasSolvedEnigma = false;

        yield return new WaitForSeconds(0.33f);
        videoPlayer.Play();
        light.SetActive(true);
        animator.SetBool("Inserting", false);
    }

    private IEnumerator EjectCd()
    {
        // Vérification supplémentaire pour s'assurer que le CD est bien en place avant d'éjecter
        if (!isCdIn) 
        {
            StopCoroutine(EjectCd());
            yield break;
        }
        if (inserting)
        {
            StopCoroutine(EjectCd());
            yield break;
        }
        animator.SetBool("Ejecting", true);

        // Attendre la fin de l'animation d'éjection
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        boxCollider.enabled = false;

        var rb = currentCd.GetComponent<Rigidbody>();
        var collider = currentCd.GetComponent<Collider>();

        collider.enabled = true;

        yield return new WaitForSeconds(1f);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        currentCd.transform.parent = null;
        currentCd = null;

        yield return new WaitForSeconds(0.15f);
        boxCollider.enabled = true;

        animator.SetBool("Ejecting", false);
        animator.SetTrigger("Reset");
    }
}*/
