using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Tunnel : MonoBehaviour {
    [Header("Relations")]
    public UnityEvent AutoSlowdown;
    public Animator _tunnelAnimator;
    public bool ignoreLever;
    
    [SerializeField] private Lever _lever;
    [SerializeField] private float _speed;
    
    // Interval @ which the tunnel gets info from the lever to interpolate
    [SerializeField] private float _speedGetInterval;
    // Interval @ which the tunnel stops automatically upon entering stations
    [SerializeField] private float _slowdownInterval;

    [Header("Debug")]
    private float _currspd;     // Current speed
    private float _spdgtime;    // Interval in secs to "get" the current speed
    private float _spd_old;     // Old speed value, start of interpolation
    private float _spd_new;     // New speed value, end of interpolation
    private float _spd_imm;     // Speed at the time of the get

    private void Start()
    {
        _lever = FindObjectOfType<Lever>();
        AutoSlowdown.AddListener(Halt);
    }

    void Update() {
        //Always keep updating the var
        _spdgtime += Time.deltaTime;
        _currspd = _lever.leverValue * _speed;
        _tunnelAnimator.SetFloat("Speed", _spd_imm);
        
        //Every x seconds, capture the current speed and interpolate w/ new speed.
        if (_spdgtime >= _speedGetInterval && ignoreLever == false) {
            _spd_old = _spd_new;
            _spd_new = _currspd;
            _spd_imm = _spd_old;

            DOTween.To(() => _spd_imm, x => _spd_imm = x, _spd_new, _speedGetInterval);
            _spdgtime = 0;
        }
    }

    private void Halt() {
        Debug.Log("<color=yellow>Halting</color>");
        ignoreLever = true;
        DOTween.To(() => _spd_imm, x => _spd_imm = x, 0, _slowdownInterval);
    }

    public void ResetInterval()
    {
        _spdgtime = _speedGetInterval;
    }
}
