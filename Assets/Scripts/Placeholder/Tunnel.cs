using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Tunnel : MonoBehaviour {
    [Header("Essentials")]
    public UnityEvent AutoSlowdown; 
    public Animator _tunnelAnimator;
    public bool ignoreLever;
    
    [SerializeField] private Lever _lever;
    [SerializeField] private float _speed;
    
    [Header("Speed")]
    // Interval @ which the tunnel gets info from the lever to interpolate
    [SerializeField] private float _speedGetInterval;
    // Interval @ which the tunnel stops automatically upon entering stations
    [SerializeField] private float _slowdownInterval;

    [Header("Debug")]
    private float _currspd;     // Current speed
    private float _spdgtime;    // Interval in secs to "get" the current speed
    private float _oldSpeed;     // Old speed value, start of interpolation
    private float _newSpeed;     // New speed value, end of interpolation
    private float _immSpeed;     // Speed at the time of the get

    private void Start()
    {
        _lever = FindObjectOfType<Lever>();
        AutoSlowdown.AddListener(Halt);
        _lever.SpeedChange.AddListener(OnSpeedChange);
    }

    void Update() {
        //Always update the variable
        _tunnelAnimator.SetFloat("Speed", _immSpeed);
    }

    private void Halt() {
        Debug.Log("<color=yellow>Halting</color>");
        _lever.Reset();
        ignoreLever = true;
        _oldSpeed = _newSpeed = 0;
        DOTween.To(() => _immSpeed, x => _immSpeed = x, _newSpeed, _slowdownInterval);
    }

    private void OnSpeedChange() {
        Debug.Log("Speed changed");
        _oldSpeed = _newSpeed;
        _newSpeed = _currspd;
        _immSpeed = _oldSpeed;
        DOTween.KillAll();
        DOTween.To(() => _immSpeed, x => _immSpeed = x, _newSpeed, _speedGetInterval);
        _spdgtime = 0;
    }

    public void ResetInterval() {
        _spdgtime = _speedGetInterval;
    }
}
