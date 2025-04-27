using UnityEngine;
using UnityEngine.Events;

public class Station : MonoBehaviour
{
    public UnityEvent Enter;
    public UnityEvent Exit;
    
    [SerializeField] private Tunnel _tunnel;
    [SerializeField] private int id;
    [SerializeField] private Enigma _enigma;
    [SerializeField] private GameObject _enigmaSolveIndicator;

    private void Start() {
        Enter.AddListener(EnterStation);
        _enigma.Solve.AddListener(OnEnigmaSolved);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Train")) {
            Enter.Invoke();
            _tunnel.AutoSlowdown.Invoke();
        }
    }

    private void EnterStation() {
        Debug.Log($"Entered station {id}");
        Debug.Log($"Starting Enigma {_enigma.id}");
        Enter.RemoveListener(EnterStation);
    }

    private void OnEnigmaSolved() {
        _tunnel.ResetInterval();
        Debug.Log("Lever reactivated");
        _tunnel.ignoreLever = false;
        _enigma.Solve.RemoveListener(OnEnigmaSolved);
    }
}
