using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    private static GameManager instance = null;
    public static GameManager Instance => instance;
    public UnityEvent End;
    
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
    
    private void Start() {
        SetBuildSettings();
        currentEnigma = 0;
    }

    public void VerifyEnigma() {
        currentEnigma++;
        Debug.Log("Ready to start next enigma");
    }

    public Enigma AssignEnigma() {
        Debug.Log($"Enigma {currentEnigma} assigned");
        if (currentEnigma == 4) {
            End.Invoke();
        }
        return _enigmas[currentEnigma];
    }

    private void SetBuildSettings() {
        Application.targetFrameRate = 90; /// IMPORTANT : FIXES FRAMERATE TO BE ACCEPTABLE IN VR
    }
}