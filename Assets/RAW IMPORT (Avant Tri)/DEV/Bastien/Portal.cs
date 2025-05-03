using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject _exitLocation;
    
    private GameObject _player;
    private GameObject _playerCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Portal triggered");
            _player.transform.position = _exitLocation.transform.position;
            _playerCamera.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
