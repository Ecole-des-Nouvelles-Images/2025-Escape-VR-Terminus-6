using UnityEngine;
using UnityEngine.Events;

public class LoopEntryTrigger : MonoBehaviour {
    public UnityEvent LoopEnter;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Train")) {
            LoopEnter.Invoke();
        }
    }
}
