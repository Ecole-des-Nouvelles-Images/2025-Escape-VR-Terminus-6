using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaSolver : MonoBehaviour {
    [SerializeField] private Enigma _enigma;

    void Start() {
        _enigma.Solve.AddListener(SolveEnigma);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enigma") && (_enigma.isSolved == false)) {
            _enigma.Solve.Invoke();
        }
    }

    private void SolveEnigma() {
        Debug.Log("Enigma solved!");
        _enigma.isSolved = true;
        _enigma.Solve.RemoveListener(SolveEnigma);
    }
}
