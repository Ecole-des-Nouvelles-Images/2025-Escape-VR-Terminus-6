using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Station : MonoBehaviour
{
    [Header("Essentials")]
    public UnityEvent Enter;                    // Station entry event
    public UnityEvent Exit;                     // Station exit event
    public UnityEvent LightsOff;
    public UnityEvent Load;
    
    [SerializeField] private Tunnel _tunnel;    // Tunnel (Tunnel will prolly be a singleton
    [SerializeField] private int id;            // Station ID
    
    [Header("Enigmas")]
    [SerializeField] private Enigma _currentEnigma;// Get events for the enigma, with minimal overhead
    [SerializeField] private bool _changeEnigmaOnExit;
    [SerializeField, CanBeNull] private GeneratorManager _generatorManager;

    [Header("Mirror Illusion -- use only for Enigma 2")]
    public bool IsMirror;                       // Used only for Enigma 2, allows mirror train code to execute
    [SerializeField] private GameObject _fakeTrain;     // Object for the mirror train
    [SerializeField] private GameObject _rotationCenter;// Used to calculate rotational symmetry
    [SerializeField] private GameObject _firstModel;
    [SerializeField] private GameObject _secondModel;
    
    [Header("Audio & Immersion")]
    [SerializeField] private Bipper _cabinBipper;
    [SerializeField] private AudioSource _speaker;      // Audio source
    [SerializeField] private AudioClip _message;        // Voice line when entering the station
    

    [Header("Debug")]
    private GameObject _playerTrain;
    private Vector3 _symVector;
    
    private void Start() {
        if(IsMirror) _fakeTrain.SetActive(false);
        _speaker = GetComponent<AudioSource>();
        _speaker.clip = _message;
        if (_secondModel) {
            _secondModel.SetActive(false);
        }
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
            GameManager.Instance.currentStation = this;
            _currentEnigma = GameManager.Instance.AssignEnigma();
            _tunnel.Halt.Invoke();
            Enter.Invoke();
            
            if (_currentEnigma) {
                _currentEnigma.Solve.AddListener(OnEnigmaSolved);
                _currentEnigma.Begin.Invoke();
                _tunnel.ignoreLever = true;
                if (_currentEnigma.Id == 2) {
                    _generatorManager.SwitchLock();
                    _generatorManager.SwitchLock();
                    this.LightsOff.AddListener(OnLightsOff);
                }
                _cabinBipper.enigma = this._currentEnigma;
                _cabinBipper.Anomaly.Invoke();
                //_generatorManager.SwitchLock(); // Done twice, because wtf
            } else { 
                _tunnel.ignoreLever = false;
                Debug.Log("No enigma here, you can continue");  
            }
        
            if (IsMirror) {
                _playerTrain = other.gameObject;
                _fakeTrain.SetActive(true);
            }
            _speaker.Play();

        }
    }

    private void OnLightsOff() {
        _firstModel.SetActive(false);
        _fakeTrain.SetActive(false);
        _secondModel.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        Exit.Invoke();
    }

    private void OnEnigmaSolved() {
        _tunnel.ignoreLever = false;
        Debug.Log("Lever reactivated after enigma");
        _currentEnigma.Solve.RemoveListener(OnEnigmaSolved);
    }
}
