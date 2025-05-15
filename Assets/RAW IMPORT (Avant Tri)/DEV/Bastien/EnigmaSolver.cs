using UnityEngine;

public class EnigmaSolver : MonoBehaviour {
    [SerializeField] private Enigma _enigma;

    void Start() {
        _enigma.Solve.AddListener(SolveEnigma);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enigma") && (_enigma.Solved == false)) {
            _enigma.Solve.Invoke();
        }
    }

    private void SolveEnigma() {
        _enigma.Solve.RemoveListener(SolveEnigma);
    }

    public void TriggerSolveEvent() {
        _enigma.Solve.Invoke();
    }
}
