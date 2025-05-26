using DEV.Scripts.Scripts_Exterior;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Tunnel : MonoBehaviour {
    [Header("Essentials")]
    public UnityEvent LoopStart;
    public UnityEvent LoopEnd;
    public UnityEvent Halt; 
    public Animator tunnelAnimator;
    public bool ignoreLever;

    [Header("Looping")]
    [SerializeField] private LoopEntryTrigger _loopEntryTrigger;

    [Header("Generation")]
    [SerializeField] private GameObject _segment;
    [SerializeField] private GameObject _container;
    [SerializeField] private float _segmentRotationStep;
    [SerializeField] private int _totalSegmentCount;
    
    [Header("Movement")]
    [SerializeField] private TrainLever _lever;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    public bool isStopped;
    
    [Header("Speed")]
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _slowdownTime;

    [Header("Debug")]
    private bool _isInSlowdown;
    private float _currentSpeed;    // Current speed
    private float _targetSpeed;     // New speed value, end of interpolation
    
    private float _currentSegment;  // For debug, to create segments

    private void Start()
    {
        tunnelAnimator = GetComponent<Animator>();
        _lever = FindObjectOfType<TrainLever>();
        _loopEntryTrigger = FindObjectOfType<LoopEntryTrigger>();
        SetupEventListeners();
        GenerateTunnel();
    }

    void Update() {
        //Always update the variable
        _targetSpeed = Mathf.Clamp01(_lever.LeverValue) * _acceleration;
        _currentSegment = transform.rotation.eulerAngles.y / _segmentRotationStep;

        if (_isInSlowdown) {
            if (_currentSpeed > 0.005) _currentSpeed -= _acceleration * Time.deltaTime / _slowdownTime;
            
            else if (_currentSpeed < 0.005) {
                _isInSlowdown = false;
                isStopped = true;
                return;
            }
        }

        if (!_isInSlowdown && isStopped == false) {
            if (_currentSpeed < _targetSpeed) {
                _currentSpeed += _acceleration * Time.deltaTime / _accelerationTime;
                _lever.Lock();
            }
            else if (_currentSpeed > _targetSpeed) {
                _currentSpeed -= _acceleration * Time.deltaTime / _accelerationTime;
            } 
        } 
        tunnelAnimator.SetFloat("Speed", _currentSpeed);

        if (isStopped) _currentSpeed = 0;
    }

    private void OnHalt() {
        _targetSpeed = 0;
        _isInSlowdown = true;
        _lever.Reset();
    }

    private void OnSpeedChange() {
        if (ignoreLever) {
            _lever.Reset();
            _lever.PlayErrorSound();
        }
        else _targetSpeed = _currentSpeed;
    }

    private void OnLoopEnter() {
        tunnelAnimator.SetBool("IsLooping", true);
    }
    
    private void SetupEventListeners() {
        _loopEntryTrigger.LoopEnter.AddListener(OnLoopEnter);
        Halt.AddListener(OnHalt);
        _lever.SpeedChange.AddListener(OnSpeedChange);
    }

    private void GenerateTunnel() {
        GameObject _sgo;
        float _yInstanceRotation = 0;
        
        for (int i = 0; i < _totalSegmentCount; i++) {
            if (i == 0 || i == 112 || i == 96){
                Debug.Log($"Cannot create tunnel segment {i}, station here.");
            } else {
                _sgo = Instantiate(_segment, _container.transform);
                _sgo.transform.rotation = Quaternion.Euler(0, _yInstanceRotation, 0);
                _sgo.GetComponent<Segment>().id = i;
            }
            _yInstanceRotation += _segmentRotationStep;
        }
    }
    
}

