using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _portal;

    [SerializeField] private Enigma _enigma;
    
    // Start is called before the first frame update
    void Start()
    {
        _portal.SetActive(false);
        SetupEventListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupEventListeners()
    {
        _enigma.Solve.AddListener(OnEnigmaSolved);
    }

    private void OnEnigmaSolved()
    {
        _back.SetActive(false);
        _portal.SetActive(true);
    }
    
}
