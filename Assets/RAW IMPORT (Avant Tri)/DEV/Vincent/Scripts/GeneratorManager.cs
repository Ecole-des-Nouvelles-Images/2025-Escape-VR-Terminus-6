using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GeneratorManager : MonoBehaviour
{
    public GeneratorLever LocalGeneratorLever;
    public Fusible LocalFusible;
    public CableAnchor LocalCableHead1;
    public CableAnchor LocalCableHead2;
    public CableAnchor LocalCableHead3;
    
    public CableHead CableHead1;
    public CableHead CableHead2;
    public CableHead CableHead3;

    public Transform CableHead1IP;
    public Transform CableHead2IP;
    public Transform CableHead3IP;
    
    public MeshRenderer LampMeshRenderer;
    public Material LampMatOn;
    public Material LampMatOff;
    
    private Animator _animator;
    private bool _locked;

    public CapsuleCollider FusibleAnchorCollider;
    
    [Header("Enigma")]
    [SerializeField] private Enigma _enigma;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openingNoise;
    [SerializeField] private AudioClip _closingNoise;

    [Header("Lights")]
    [SerializeField] private Light _amogusLight;
    [SerializeField] private Light _leverLight;
    [SerializeField] private Light _cabinLight;
    [SerializeField] private Animator _animatorController;


    private BoxCollider _localCableHead1Collider;
    private BoxCollider _localCableHead2Collider;
    private BoxCollider _localCableHead3Collider;
    
    public bool GeneratorOk { get; private set; }
    private bool _amogusOk;
    private bool _cableDisconnected;
    private bool _resetDone;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _locked = false;
        _amogusLight.enabled = false; _leverLight.enabled = false;
        _localCableHead1Collider = LocalCableHead1.transform.GetComponent<BoxCollider>();
        _localCableHead2Collider = LocalCableHead2.transform.GetComponent<BoxCollider>();
        _localCableHead3Collider = LocalCableHead3.transform.GetComponent<BoxCollider>();
        _localCableHead1Collider.enabled = false;
        _localCableHead2Collider.enabled = false;
        _localCableHead3Collider.enabled = false;
        CableHead1.LockHead(); 
        CableHead2.LockHead();
        CableHead3.LockHead();
        _cableDisconnected = false;
    }

    private void Start() {
        //SwitchLock();
    }

    private void Update()
    {
        UpdateGeneratorOk();
        //VerifyLamp();
        CheckLights();
    }

    private void UpdateGeneratorOk()
    {
        // Vérifie les booléens dans les objets associés
        //bool fusibleOk = LocalFusible       != null && LocalFusible.FusibleOk;
        bool fusibleOk = true;
        bool cableHead1Ok = LocalCableHead1 != null && LocalCableHead1.CableOk;
        bool cableHead2Ok = LocalCableHead2 != null && LocalCableHead2.CableOk;
        bool cableHead3Ok = LocalCableHead3 != null && LocalCableHead3.CableOk;
        _amogusOk = (cableHead1Ok && cableHead2Ok && cableHead3Ok);
        // Met à jour GeneratorOk en fonction des booléens
        GeneratorOk = (LocalGeneratorLever.LeverActivated == false && _amogusOk);
        
        if (GeneratorOk) {
            _enigma.Solve.Invoke();
            LocalGeneratorLever.LockLeverGenerator();
            GameManager.Instance.currentStation.LightsOff.Invoke();
        }
    }

    /*private void VerifyLamp() {
        if (GeneratorOk) {
            LampOn();
        }
        else {
            LampOff();
        }
    }*/

    private void LampOn() {
        if (LampMeshRenderer.material != LampMatOn) {
            LampMeshRenderer.material = LampMatOn;
        }
    }
    
    private void LampOff() {
        if (LampMeshRenderer.material != LampMatOff) {
            LampMeshRenderer.material = LampMatOff;
        }
    }

    private void CheckLights() {
        if (LocalGeneratorLever.LeverActivated && _amogusOk == false) {
            _leverLight.enabled = false;
            _amogusLight.enabled = true;
            _cabinLight.enabled = false;
            _animatorController.SetBool("CubeActivated", true);
            LampOff();
            if (_cableDisconnected == false && _resetDone == false) {
                CableHead1.gameObject.transform.position = CableHead1IP.transform.position;
                CableHead1.UnlockHead();
                CableHead2.gameObject.transform.position = CableHead2IP.transform.position;
                CableHead2.UnlockHead();
                CableHead3.gameObject.transform.position = CableHead3IP.transform.position;
                CableHead3.UnlockHead();
                _cableDisconnected = true;
                _localCableHead1Collider.enabled = true;
                _localCableHead2Collider.enabled = true;
                _localCableHead3Collider.enabled = true;
                _resetDone = true;
            }
        } else if (LocalGeneratorLever.LeverActivated == false && _amogusOk) {
            _leverLight.enabled = true;
            _amogusLight.enabled = false;
            _cabinLight.enabled = true;
            _animatorController.SetBool("CubeActivated", false);
            LampOn();
        }
    }

    [ContextMenu("Switch Lock")]
    public void SwitchLock() {
        if (_locked) {
            LocalGeneratorLever.UnlockLeverGenerator();
            FusibleAnchorCollider.enabled = true;
            /*CableHead1.UnlockHead();
            CableHead2.UnlockHead();
            CableHead3.UnlockHead();
            CableHead1.UnlockHead();
            CableHead2.UnlockHead();
            CableHead3.UnlockHead();*/
            _animator.SetTrigger("OpenGenerator");
            _audioSource.clip = _openingNoise;
            _audioSource.Play();
            //_amogusLight.enabled = true;
            _leverLight.enabled = true;
        }
        else {
            LocalGeneratorLever.LockLeverGenerator();
            FusibleAnchorCollider.enabled = false;
            CableHead1.LockHead();
            CableHead2.LockHead();
            CableHead3.LockHead();
            CableHead1.LockHead();
            CableHead2.LockHead();
            CableHead3.LockHead();
            _locked = true;
        }
    }

    public void PlayOpenSound() {
        _audioSource.clip = _openingNoise;
        _audioSource.Play();
    }
    
}