using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Enigma Management & tracking")]
    [SerializeField] private List<Enigma> _enigmas;

    private int currentEnigma = 0;
    
    private void Start() {
        SetupEventListeners();
        currentEnigma = 0;
    }
    
    private UnityAction OnEnigmaSolved(Enigma e)
    {
        if (e.isSolved) return null;
        
        e.Solve.RemoveListener(() => OnEnigmaSolved(e));
        return () => currentEnigma = _enigmas.IndexOf(e);
    }

    private void SetupEventListeners() {
        foreach (Enigma e in _enigmas) {
            e.Solve.AddListener(OnEnigmaSolved(e));
        }
    }
}
