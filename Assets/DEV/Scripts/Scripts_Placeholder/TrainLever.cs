using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

    public class TrainLever : MonoBehaviour {
        [Header("Essentials")]
        public float LeverValue;
        public UnityEvent SpeedChange;
        [SerializeField] private HingeJoint _hingeJoint;
        [SerializeField] private XRGrabInteractable _xrGrabInteractable;
        
        [Header("Values")]
        [SerializeField] private float _leverMargin;
        [SerializeField] private float _maxInteractionDistance;
        
        [Header("Audio & Immersion")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _errorSound;
        
        [Header("Help visuals")]
        [SerializeField] private GameObject _guideArrow;
        
        [Header("Debug")]
        private float _leverRotation;   //Self-explanatory
        private float _min, _max, _mid; //Minimum, maximum and middle rotation
        private float _lowMid, _highMid;   //Values calculated from the middle for padding
        public bool isMovable;        //Is the lever locked? Not the same as ignoreLever
    
        public void Unlock() {
            isMovable = true;
        }
    
        public void Lock() {
            Debug.Log("Entering Lock");
            isMovable = false;
            if (_leverRotation <= _mid) {
                transform.rotation = Quaternion.Euler(new Vector3(_min, 0f, 0f));
                LeverValue = 0f;
            } else if (_leverRotation > _mid || (_leverRotation + _leverMargin) >= _max) {
                transform.rotation = Quaternion.Euler(new Vector3(_max, 0f, 0f));
                LeverValue = 1f;
            }
            SpeedChange.Invoke();
        }

        public void EnableHelpVisuals() { _guideArrow.SetActive(true); }
        public void DisableHelpVisuals() { _guideArrow.SetActive(false); }
    
        public void Reset() {
            transform.rotation = Quaternion.Euler(_min, 0, 0);
            LeverValue = 0f;
        }

        public void SetToMax() {
            transform.rotation = Quaternion.Euler(new Vector3(_max, 0, 0));
            LeverValue = 1f;
        }
        
        private void Start() {
            _min = _hingeJoint.limits.min;
            _max = _hingeJoint.limits.max;
            _mid = _min + ((_max - _min) / 2);
            _lowMid = _mid - _leverMargin;
            _highMid = _mid + _leverMargin;
            _audioSource = GetComponent<AudioSource>();
            _guideArrow.SetActive(false); // Disable guide arrow on start
        }
    
        void Update() {
            if (isMovable) {
                _leverRotation = transform.rotation.eulerAngles.x;
                this.transform.rotation = Quaternion.Euler(new Vector3(_leverRotation, 0, 0));
            }
        }
        

        public void PlayErrorSound() {
            _audioSource.clip = _errorSound;
            _audioSource.Play();
        }
    }