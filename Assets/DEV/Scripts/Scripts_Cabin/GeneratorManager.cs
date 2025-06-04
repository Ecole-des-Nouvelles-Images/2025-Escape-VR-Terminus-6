using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DEV.Scripts.Scripts_Cabin {
    public class GeneratorManager : MonoBehaviour
    {
        public GeneratorLever LocalGeneratorLever;
        public Fusible LocalFusible;
        public CableAnchor CableAnchor1;
        public CableAnchor CableAnchor2;
        public CableAnchor CableAnchor3;
    
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
        [SerializeField] private Animator _lightsAnimator;
        public ParticleSystem _sparks;
        
        private BoxCollider _cableAnchorColl1;
        private BoxCollider _cableAnchorColl2;
        private BoxCollider _cableAnchorColl3;
        private float _pullLerpTime;
        private float _pbc;
        
        public bool GeneratorOk { get; private set; }
        private bool _amogusOk;
        private bool _cableDisconnected;
        private bool _resetDone;

        private bool _c1ok, _c2ok, _c3ok;

        private List<CableHead> _cableHeads;

        [SerializeField] int lerpId;
        [SerializeField] int lerpLimit;
        
        //Putain c'est du sale
        private void Awake() {
            _animator = GetComponent<Animator>();
            _cableHeads = new List<CableHead>();
            _cableAnchorColl1 = CableAnchor1.transform.GetComponent<BoxCollider>();
            _cableAnchorColl2 = CableAnchor2.transform.GetComponent<BoxCollider>();
            _cableAnchorColl3 = CableAnchor3.transform.GetComponent<BoxCollider>();
            _cableHeads.Add(CableHead1);
            _cableHeads.Add(CableHead2);
            _cableHeads.Add(CableHead3);
            _locked = false;
            _amogusLight.enabled = false; _leverLight.enabled = false;
            CableHead1.Connected.AddListener(OnCableConnect);
            CableHead2.Connected.AddListener(OnCableConnect);
            CableHead3.Connected.AddListener(OnCableConnect);
            _cableAnchorColl1.enabled = false;
            _cableAnchorColl2.enabled = false;
            _cableAnchorColl3.enabled = false;
            CableHead1.LockHead(); 
            CableHead2.LockHead();
            CableHead3.LockHead();
            
            _cableDisconnected = false;
        }

        private void Start() {
            _pullLerpTime = Time.deltaTime;
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
            //bool fusibleOk = true;
            _c1ok = CableAnchor1.CableOk;
            _c2ok = CableAnchor2.CableOk;
            _c3ok = CableAnchor3.CableOk;
            _amogusOk = (_c1ok && _c2ok && _c3ok);
            // Met à jour GeneratorOk en fonction des booléens
            GeneratorOk = (LocalGeneratorLever.LeverActivated == false && _amogusOk);
        
            if (GeneratorOk) {
                _animatorController.SetBool("CubeActivated", false);
                //_enigma.Solve.Invoke();
                SwitchLock();
                GameManager.Instance.currentStation.LightsOff.Invoke();
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

        private void CheckLights() {
            if (LocalGeneratorLever.LeverActivated && _amogusOk == false) {
                LocalGeneratorLever.LockLeverGenerator();
                _leverLight.enabled = false;
                _lightsAnimator.SetTrigger("Switch");
                _cabinLight.enabled = false;
                _animatorController.SetBool("CubeActivated", true);
                LampOff();
                if (_cableDisconnected == false && _resetDone == false) {
                    //CableHead1.gameObject.transform.position = CableHead1IP.transform.position;
                    //CableHead2.gameObject.transform.position = CableHead2IP.transform.position;
                    //CableHead3.gameObject.transform.position = CableHead3IP.transform.position;
                    StartCoroutine(PullCables());
                    _sparks.Play();
                    CableHead1.UnlockHead();
                    CableHead2.UnlockHead();
                    CableHead3.UnlockHead();
                    // CableHead1.DetachFromAnchor();
                    // CableHead2.DetachFromAnchor();
                    // CableHead3.DetachFromAnchor();
                    // CableHead1.ReturnToBase();
                    // CableHead2.ReturnToBase();
                    // CableHead3.ReturnToBase();
                    _cableDisconnected = true;
                    _cableAnchorColl1.enabled = true;
                    _cableAnchorColl2.enabled = true;
                    _cableAnchorColl3.enabled = true;
                    _resetDone = true;
                }
            } else if (LocalGeneratorLever.LeverActivated && _amogusOk) {
                LocalGeneratorLever.UnlockLeverGenerator();
                _leverLight.enabled = true;
                _amogusLight.enabled = false;
                _cabinLight.enabled = true;
                LampOn();
            }
        }

        [ContextMenu("Switch Lock")]
        public void SwitchLock() {
            if (_locked) {
                LocalGeneratorLever.UnlockLeverGenerator();
                FusibleAnchorCollider.enabled = true;
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

        private void OnCableConnect()
        {
            foreach (CableHead _ch in _cableHeads)
            {
                if (_ch.tempCableAnchor == null) return;
                    
                _ch.tempCableAnchor.CableOk = false;
                _ch.tempCableAnchor.VerifyColor(_ch.ConnectedMaterial);
            }
        }

        private IEnumerator PullCables() {
            CableHead1.DetachFromAnchor();
            CableHead2.DetachFromAnchor();
            CableHead3.DetachFromAnchor();
            CableHead1.ReturnToBase();
            CableHead2.ReturnToBase();
            CableHead3.ReturnToBase();
            yield return new WaitForSeconds(_pullLerpTime);
            lerpId += 1;
            if (lerpId < lerpLimit) {
                StartCoroutine(PullCables());
            }
            else {
                StopAllCoroutines();
                yield break;
            }

        }
    }
}