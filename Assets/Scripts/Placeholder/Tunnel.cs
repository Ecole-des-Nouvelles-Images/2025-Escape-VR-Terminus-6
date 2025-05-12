using DG.Tweening;
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
    
    [Header("Movement")]
    [SerializeField] private TrainLever _lever;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    
    [Header("Speed")]
    [SerializeField] private float _accelerationTime;
    [SerializeField] private float _slowdownTime;

    [Header("Debug")]
    private bool _isInSlowdown;
    private float _currentSpeed;     // Current speed
    private float _targetSpeed;     // New speed value, end of interpolation

    private void Start()
    {
        tunnelAnimator = GetComponent<Animator>();
        _lever = FindObjectOfType<TrainLever>();
        _loopEntryTrigger = FindObjectOfType<LoopEntryTrigger>();
        SetupEventListeners();
    }

    void Update() {
        //Always update the variable
        _targetSpeed = Mathf.Clamp(_lever.LeverValue, 0f, 1f) * _acceleration;

        if (_isInSlowdown) {
            if (_currentSpeed > 0) _currentSpeed -= _acceleration * Time.deltaTime / _slowdownTime;
            else {
                _isInSlowdown = false;
                return;
            }
        }

        if (!_isInSlowdown) {
            if (_currentSpeed < _targetSpeed) {
                _currentSpeed += _acceleration * Time.deltaTime / _accelerationTime;
            }
            else if (_currentSpeed > _targetSpeed) {
                _currentSpeed -= _acceleration * Time.deltaTime / _accelerationTime;
            } 
        } 
        tunnelAnimator.SetFloat("Speed", _currentSpeed);
    }

    private void OnHalt() {
        _targetSpeed = 0;
        _isInSlowdown = true;
        _lever.Reset();
    }

    private void OnSpeedChange() {
        if (ignoreLever) _lever.Reset();
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
}

