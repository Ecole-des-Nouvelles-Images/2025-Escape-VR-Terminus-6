using DG.Tweening;
using UnityEngine;

public class Tunnel : MonoBehaviour {
    [SerializeField] private Lever _lever;
    [SerializeField] private Animator _tunnelAnimator;
    [SerializeField] private float _speed;
    
    // Interval @ which the tunnel gets info from the lever to interpolate
    [SerializeField] private float _speedGetInterval;

    private float _currSpeed;
    private float _speedIntervalTime;
    private float _spd_old, _spd_new, _spd_imm;
    
    void Update() {
        //Always keep updating the var
        _speedIntervalTime += Time.deltaTime;
        _currSpeed = _lever.leverValue * _speed;
        _tunnelAnimator.SetFloat("Speed", _spd_imm);
        
        //Every x seconds, capture the current speed and interpolate w/ new speed.
        if (_speedIntervalTime >= _speedGetInterval) {
            _spd_old = _spd_new;
            _spd_new = _currSpeed;

            _spd_imm = _spd_old;

            DOTween.To(() => _spd_imm, x => _spd_imm = x, _spd_new, _speedGetInterval);
            _speedIntervalTime = 0;
        }
        
        transform.Translate(0, 0, _spd_imm * _speed * Time.deltaTime);
    }

    private void Interval() {
        
    }

    public void Halt() {
        DOTween.To(() => _spd_imm, x => _spd_imm = x, 0, _speedGetInterval);
    }
}
