using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour {

    [SerializeField] private Enigma _enigma;
    // Start is called before the first frame update
    void Start()
    {
        _enigma.Solve.AddListener(OnEnigmaSolved);
    }

    public void OnEnigmaSolved() {
        GetComponent<Animator>().SetTrigger("OpenDoors");
    }
}
