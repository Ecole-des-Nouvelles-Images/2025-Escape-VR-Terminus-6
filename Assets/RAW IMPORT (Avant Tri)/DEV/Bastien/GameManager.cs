using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    private static GameManager instance = null;
    public static GameManager Instance => instance;
    
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return; 
        } else { instance = this; }
        DontDestroyOnLoad(this.gameObject);
    }
    
    
    [Header("Enigma Management & tracking")]
    [SerializeField] private List<Enigma> _enigmas;
    public Station currentStation;

    public int currentEnigma = 0;
    // [SerializeField] private int currentEnigmaBackup = 0;

    
    private void Start() {
        currentEnigma = 0;
    }

    public void VerifyEnigma(Enigma enigma)
    {
        currentEnigma++;
        Debug.Log("Increasing Enigma by 1");
    }

    public Enigma AssignEnigma() {
        Debug.Log($"Enigma {currentEnigma} assigned");
        return _enigmas[currentEnigma];
    }
}