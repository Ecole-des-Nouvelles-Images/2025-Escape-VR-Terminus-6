using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Enigma Management & tracking")]
    [SerializeField] private List<Enigma> _enigmas;

    [SerializeField] private int currentEnigma = 0;
    [SerializeField] private int currentEnigmaBackup = 0;
    
    private void Start() {
        currentEnigma = 0;
    }

    public void VerifyEnigma(Enigma enigma) {
        currentEnigmaBackup = enigma.Id;
        currentEnigma = currentEnigmaBackup; //were cooked
        enigma.Solved = true;
    }
}