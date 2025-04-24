using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhoneManager : MonoBehaviour
{
    public Transform phoneTransform;
    public Transform parentTransform;
    public Transform anchorTransform; // L'ancre pour "snap" le téléphone
    public LineRenderer lineRenderer;
    public float maxDistance = 1.0f;
    public int segmentCount = 10; // Nombre de segments dans le fil
    public float gravityEffect = 0.5f; // Effet de gravité sur la ligne
    public float anchorRange = 0.1f; // Portée de l'ancre
    public MeshRenderer snapIndicator; // Indicateur visuel pour le snap
    public AudioCode audioCode; // Référence au script AudioCode

    private Vector3 initialPhonePosition;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody phoneRigidbody;
    private bool isOnBase = true;
    private Vector3[] segments;

    void Start()
    {
        initialPhonePosition = phoneTransform.localPosition;
        grabInteractable = phoneTransform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        phoneRigidbody = phoneTransform.GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        lineRenderer.positionCount = segmentCount;
        segments = new Vector3[segmentCount];

        // Ajouter un SphereCollider trigger à l'ancre
        SphereCollider anchorCollider = anchorTransform.gameObject.AddComponent<SphereCollider>();
        anchorCollider.isTrigger = true;
        anchorCollider.radius = anchorRange;
    }

    void Update()
    {
        UpdateLineRenderer();

        if (grabInteractable.isSelected)
        {
            CheckDistance();
        }

        // Vérifier si le téléphone est dans la zone de l'ancre
        if (Vector3.Distance(phoneTransform.position, anchorTransform.position) < anchorRange)
        {
            snapIndicator.enabled = !isOnBase;
        }
        else
        {
            snapIndicator.enabled = false;
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Détacher le téléphone de l'ancre
        if (phoneTransform.parent == anchorTransform)
        {
            phoneTransform.parent = null;
        }

        phoneRigidbody.useGravity = true;
        isOnBase = false;

        // Désactiver les contraintes
        phoneRigidbody.constraints = RigidbodyConstraints.None;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Vérifier si le téléphone est proche de l'ancre
        if (Vector3.Distance(phoneTransform.position, anchorTransform.position) < anchorRange)
        {
            SnapToAnchor();
        }
        else
        {
            // Assurez-vous que le téléphone utilise la gravité s'il n'est pas snappé
            phoneRigidbody.useGravity = true;
        }
    }

    void SnapToAnchor()
    {
        isOnBase = true;
        phoneRigidbody.useGravity = false;
        phoneRigidbody.velocity = Vector3.zero;
        phoneRigidbody.angularVelocity = Vector3.zero;
        phoneTransform.position = anchorTransform.position;
        phoneTransform.rotation = anchorTransform.rotation;

        // Attacher le téléphone à l'ancre
        phoneTransform.parent = anchorTransform;

        // Activer les contraintes pour bloquer le téléphone
        phoneRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Arrêter l'audio
        if (audioCode != null)
        {
            audioCode.StopAudio();
        }
    }

    void UpdateLineRenderer()
    {
        Vector3 startPoint = parentTransform.position;
        Vector3 endPoint = phoneTransform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 segmentPosition = Vector3.Lerp(startPoint, endPoint, t);

            // Appliquer un effet de gravité sur les segments
            segmentPosition.y -= Mathf.Sin(t * Mathf.PI) * gravityEffect;

            segments[i] = segmentPosition;
            lineRenderer.SetPosition(i, segmentPosition);
        }
    }

    void CheckDistance()
    {
        if (Vector3.Distance(phoneTransform.position, parentTransform.position) > maxDistance)
        {
            grabInteractable.selectExited.Invoke(new SelectExitEventArgs());
        }
    }
}