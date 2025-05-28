using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enigma : MonoBehaviour { 
    public UnityEvent Solve;
    public UnityEvent Begin;
    public bool Solved;
    public bool isAnomaly;
    public int Id;
    public string code;
    
    private void Start()
    {
        this.Solve.AddListener(OnSolve); 
    }

    private void OnSolve()
    {
        this.Solved = true;
        SolveValidation();
    }

    private void SolveValidation() {
        FindObjectOfType<GameManager>().VerifyEnigma(this);
        Debug.Log("Try to solve " + this.gameObject);
        this.Solve.RemoveListener(OnSolve);
    }
}