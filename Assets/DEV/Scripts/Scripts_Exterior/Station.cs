using System;
using DEV.Scripts.Scripts_Cabin;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Station : MonoBehaviour
{
    [Header("Essentials")]
    public UnityEvent Enter;                    // Station entry event
    public UnityEvent Exit;                     // Station exit event
    public UnityEvent LightsOff;
    public UnityEvent Report;
    public UnityEvent Load;

    [Header("Radio")]
    public string code;
    private bool _reported;
    
    [SerializeField] private Tunnel _tunnel;    // Tunnel (Tunnel will prolly be a singleton
    [SerializeField] private int id;            // Station ID
    [SerializeField] private TrainLever _lever;
    
    [Header("Enigmas")]
    [SerializeField] private Enigma _currentEnigma;// Get events for the enigma, with minimal overhead
    [SerializeField] private bool _changeEnigmaOnExit;
    [SerializeField, CanBeNull] private GeneratorManager _generatorManager;
    [SerializeField] private XRButton _doorButton;

    [Header("Radio -- Use only for Enigma 1")]
    public NumberManager _radioNumberManager;
    
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
    private bool _stationExited;
    
    private void Start() {
        if(IsMirror) _fakeTrain.SetActive(false);
        _speaker = GetComponent<AudioSource>();
        if(_doorButton != null) _doorButton.GetComponent<BoxCollider>().enabled = true;
        _speaker.clip = _message;
        if (_secondModel) {
            _secondModel.SetActive(false);
        }
        Report.AddListener(OnReport);
    }

    private void Update() {
        if (IsMirror && _playerTrain) {
            _symVector = _playerTrain.transform.position - _rotationCenter.transform.position;
            _fakeTrain.transform.position = new Vector3((_symVector.x * -1) + _rotationCenter.transform.position.x,
                _symVector.y, (_symVector.z * -1) + _rotationCenter.transform.position.z);
        }

        if (IsMirror) {
            if (_generatorManager.GeneratorOk && _reported) _currentEnigma.Solve.Invoke();
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Train")) {

            _stationExited = false;
            _speaker.Play();
            if(_doorButton != null)_doorButton.GetComponent<BoxCollider>().enabled = false;
            GameManager.Instance.currentStation = this;
            _currentEnigma = GameManager.Instance.AssignEnigma();
            code = _currentEnigma.code;
            _radioNumberManager.ChangeStation.Invoke();
            _tunnel.Halt.Invoke();
            Enter.Invoke();
            
            if (!_currentEnigma) {
                _tunnel.ignoreLever = false;
                Debug.Log("No enigma here, you can continue");
            }
            else {
                _currentEnigma.Solve.AddListener(OnEnigmaSolved);
                _currentEnigma.Begin.Invoke();
                _tunnel.ignoreLever = true;

                switch (_currentEnigma.Id) {
                    case 0: {
                        /*_radioNumberManager.enabled = false;
                        foreach (Transform child in _radioNumberManager.transform) {
                            if (child.GetComponent<BoxCollider>()){
                                child.GetComponent<BoxCollider>().enabled = false;
                            }
                        }*/
                        break;
                    }
                    case 1: {
                        _radioNumberManager.enabled = true;
                        foreach (Transform child in _radioNumberManager.transform) {
                            if (child.GetComponent<BoxCollider>()) {
                                child.GetComponent<BoxCollider>().enabled = true;
                            }
                        }
                        break;
                    }
                    case 2: {
                        _generatorManager.SwitchLock();
                        _generatorManager.SwitchLock();
                        this.LightsOff.AddListener(OnLightsOff);
                        break;
                    }
                    case 3: {
                        _doorButton.GetComponent<BoxCollider>().enabled = true;
                        break;
                    }
                }

                _cabinBipper.enigma = this._currentEnigma;
                _cabinBipper.Anomaly.Invoke();
                //_generatorManager.SwitchLock(); // Done twice, because wtf
            }

            if (IsMirror) {
                _playerTrain = other.gameObject;
                _fakeTrain.SetActive(true);
            }
        }
    }

    private void OnLightsOff() {
        _firstModel.SetActive(false);
        _fakeTrain.SetActive(false);
        _secondModel.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        if (_stationExited) {
            return;
        }
        else {
            GameManager.Instance.VerifyEnigma();
            _stationExited = true;
            Debug.Log("Station exited");
            Exit.Invoke();
            _lever.SetToMax();
            _lever.Lock();
            _lever.Lock();
        }
        
    }

    private void OnEnigmaSolved() {
        _tunnel.ignoreLever = false;
        _tunnel.isStopped = false;
        Debug.Log("Lever reactivated after enigma");
        _currentEnigma.Solve.RemoveListener(OnEnigmaSolved);
    }

    private void OnReport() {
        _reported = true;
    }
}