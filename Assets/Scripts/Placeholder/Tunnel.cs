using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Tunnel : MonoBehaviour {
    [Header("Essentials")]
    public UnityEvent LoopStart;
    public UnityEvent LoopEnd;
    public UnityEvent AutoSlowdown; 
    public Animator _tunnelAnimator;
    public bool ignoreLever;

    [Header("Looping")]
    [SerializeField] private LoopEntryTrigger _loopEntryTrigger;
    
    [Header("Movement")]
    [SerializeField] private TrainLever _lever;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    
    [Header("Speed")]
    // Interval @ which the tunnel gets info from the lever to interpolate
    [SerializeField] private float _accelerationTime;
    // Interval @ which the tunnel stops automatically upon entering stations
    [FormerlySerializedAs("_slowdownInterval"),SerializeField] private float _slowdownTime;

    [Header("Debug")]
    private bool _isInSlowdown;
    private float _currentSpeed;     // Current speed
    private float _targetSpeed;     // New speed value, end of interpolation

    private void Start()
    {
        _lever = FindObjectOfType<TrainLever>();
        _loopEntryTrigger = FindObjectOfType<LoopEntryTrigger>();
        _loopEntryTrigger.LoopEnter.AddListener(OnLoopEnter);
        AutoSlowdown.AddListener(Halt);
        _lever.SpeedChange.AddListener(OnSpeedChange);
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
        _tunnelAnimator.SetFloat("Speed", _currentSpeed);
    }

    private void Halt() {
        _targetSpeed = 0;
        _isInSlowdown = true; ignoreLever = true;
        _lever.Reset();
    }

    private void OnSpeedChange() {
        if(ignoreLever) return;
        _targetSpeed = _currentSpeed;
    }

    private void OnLoopEnter() {
        _tunnelAnimator.SetBool("IsLooping", true);
    }
}
