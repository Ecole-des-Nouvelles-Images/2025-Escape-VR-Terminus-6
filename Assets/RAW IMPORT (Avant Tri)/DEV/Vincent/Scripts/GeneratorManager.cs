using System;
using UnityEngine;

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
    [SerializeField] private GameObject _amogusLight, _leverLight;
    
    public bool GeneratorOk { get; private set; }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _locked = false;
        _amogusLight.SetActive(false); _leverLight.SetActive(false);
    }

    private void Start() {
        //SwitchLock();
    }

    private void Update()
    {
        UpdateGeneratorOk();
        VerifyLamp();
    }

    private void UpdateGeneratorOk()
    {
        // Vérifie les booléens dans les objets associés
        bool leverOk = LocalGeneratorLever  != null && LocalGeneratorLever.LeverActivated;
        bool fusibleOk = LocalFusible       != null && LocalFusible.FusibleOk;
        bool cableHead1Ok = LocalCableHead1 != null && LocalCableHead1.CableOk;
        bool cableHead2Ok = LocalCableHead2 != null && LocalCableHead2.CableOk;
        bool cableHead3Ok = LocalCableHead3 != null && LocalCableHead3.CableOk;

        // Met à jour GeneratorOk en fonction des booléens
        GeneratorOk = leverOk && fusibleOk && cableHead1Ok && cableHead2Ok && cableHead3Ok;

        if (GeneratorOk)
        {
            _enigma.Solve.Invoke();
        }
    }

    private void VerifyLamp() {
        if (GeneratorOk) {
            LampOn();
        }
        else {
            LampOff();
        }
    }

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

    [ContextMenu("Switch Lock")]
    public void SwitchLock() {
        if (_locked) {
            LocalGeneratorLever.UnlockLeverGenerator();
            FusibleAnchorCollider.enabled = true;
            CableHead1.UnlockHead();
            CableHead2.UnlockHead();
            CableHead3.UnlockHead();
            CableHead1.UnlockHead();
            CableHead2.UnlockHead();
            CableHead3.UnlockHead();
            _animator.SetTrigger("OpenGenerator");
            _audioSource.clip = _openingNoise;
            _audioSource.Play();
            _amogusLight.SetActive(true);
            _leverLight.SetActive(true);
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