using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enigma : MonoBehaviour { 
    public UnityEvent Solve;
    public UnityEvent Begin;
    public bool isSolved;
    
    private void Start()
    {
       this.Solve.AddListener(OnSolve); 
    }

    private void OnSolve()
    {
        this.isSolved = true;
    }
}
