using UnityEngine;
using UnityEngine.UI;

namespace DEV.Scripts.Scripts_Transitions_and_Ending {
    public class Ending : MonoBehaviour {
        private Image _image;
        private Animator _animator;
        [SerializeField] private GameObject _menuButton;
    
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
}
