using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour {
    private Image _image;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start() {
        _image = GetComponentInChildren<Image>();
        _animator = GetComponentInChildren<Animator>();
        GameManager.Instance.End.AddListener(OnGameEnd);
    }

    private void OnGameEnd() {
        _animator.SetTrigger("Ending");
    }
}
