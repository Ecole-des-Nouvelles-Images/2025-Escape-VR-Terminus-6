using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationIllusion : MonoBehaviour
{
    [SerializeField] private GameObject  _realTeleportPoint, _fakeTeleportPoint;
    [SerializeField] private GameObject _realStation, _fakeStation;

    private GameObject _realCenter, _fakeCenter;

    private Vector3 _posDelta;
    
    // Start is called before the first frame update
    void Start()
    {
        _realCenter = _realStation.GetComponentInChildren<Center>().gameObject;
        _fakeCenter = _fakeStation.GetComponentInChildren<Center>().gameObject; 
        
        _posDelta = _realTeleportPoint.transform.position - _realCenter.transform.position;
        
        _fakeCenter.transform.localPosition = -_posDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
