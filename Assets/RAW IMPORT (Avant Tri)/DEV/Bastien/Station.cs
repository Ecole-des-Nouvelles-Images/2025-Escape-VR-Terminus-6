using UnityEngine;
using UnityEngine.Events;

public class Station : MonoBehaviour
{
    [Header("Essentials")]
    public UnityEvent Enter;                    // Station entry event
    public UnityEvent Exit;                     // Station exit event
    public bool IsMirror;                       // Used only for Enigma 2, allows mirror train code to execute
    [SerializeField] private Tunnel _tunnel;    // Tunnel (Tunnel will prolly be a singleton
    [SerializeField] private int id;            // Station ID
    [SerializeField] private Enigma _enigma;    // Get events for the enigma, with minimal overhead

    [Header("Mirror Illusion -- use only for Enigma 2")]
    [SerializeField] private GameObject _fakeTrain;     // Object for the mirror train
    [SerializeField] private GameObject _rotationCenter;// Used to calculate rotational symmetry
    
    [Header("Audio & Immersion")]
    [SerializeField] private AudioSource _speaker;      // Audio source
    [SerializeField] private AudioClip _message;        // Voice line when entering the station

    [Header("Debug")]
    private GameObject _playerTrain;
    private Vector3 _symVector;
    
    private void Start() {
        if(IsMirror) _fakeTrain.SetActive(false);
        
        _speaker = GetComponent<AudioSource>();
        _speaker.clip = _message;
        
        Enter.AddListener(OnStationEnter);
        if(_enigma) _enigma.Solve.AddListener(OnEnigmaSolved);
    }

    private void Update() {
        if (IsMirror && _playerTrain) {
            _symVector = _playerTrain.transform.position - _rotationCenter.transform.position;
            _fakeTrain.transform.position = new Vector3((_symVector.x * -1) + _rotationCenter.transform.position.x,
                _symVector.y, (_symVector.z * -1) + _rotationCenter.transform.position.z);
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Train")) {
            _playerTrain = other.gameObject;
            Debug.Log("trigger hit");
            _tunnel.Halt.Invoke();
            Enter.Invoke();
        }
    }

    private void OnStationEnter() {
        if (_enigma) {
            _tunnel.ignoreLever = true;
            _enigma.Begin.Invoke();
            Debug.Log($"Starting Enigma {_enigma.id}");
        } else { 
            _tunnel.ignoreLever = false;
            Debug.Log("No enigma here");  
        }
        
        if (IsMirror) {
            _fakeTrain.SetActive(true);
        }
        _speaker.Play();
    }

    private void OnEnigmaSolved() {
        _tunnel.ignoreLever = false;
        Debug.Log("Lever reactivated");
        _enigma.Solve.RemoveListener(OnEnigmaSolved);
    }
}
