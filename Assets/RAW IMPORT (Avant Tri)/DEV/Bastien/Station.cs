using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Station : MonoBehaviour {
    public UnityEvent Enter;
    public UnityEvent Exit;
    
    [SerializeField] private int id;
    [SerializeField] private Enigma _enigma;
    [SerializeField] private GameObject _enigmaSolveIndicator;

    private void Start() {
        Enter.AddListener(EnterStation);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Train")) {
            Enter.Invoke();
        }
    }

    private void EnterStation() {
        Debug.Log($"Entered station {id}");
        Debug.Log($"Starting Enigma {_enigma.id}");
        Enter.RemoveListener(EnterStation);
    }
}
