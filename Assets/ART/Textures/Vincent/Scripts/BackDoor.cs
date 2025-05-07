using UnityEngine;

public class BackDoor : MonoBehaviour {
    public bool DoorLock;
    private Animator _animator;
    private bool _doorOpen;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        _animator.SetBool("DoorOpened", _doorOpen);
    }

    [ContextMenu("SWITCH")]
    public void SwitchOpenDoor() {
        if (!DoorLock) _doorOpen = !_doorOpen;
    }
}