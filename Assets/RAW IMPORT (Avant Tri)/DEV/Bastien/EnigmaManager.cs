using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaManager : MonoBehaviour {
    
    [Header("Enigmas")]
    [SerializeField] private List<Enigma> _enigmas;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Enigma e in _enigmas) {
            e.Solve.AddListener(OnEnigmaSolved);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnigmaSolved() {
        
    }
}
