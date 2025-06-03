using UnityEngine;

public class Ghost : MonoBehaviour {
   [SerializeField] private Animator _ghostAnimator;
   [SerializeField] private Enigma _enigma;
   [SerializeField] private AudioSource _audioSource;
   [SerializeField] private AudioClip _ghostVoice;
   
   private void Start() {
      _audioSource.clip = _ghostVoice;
      _audioSource.Play();
      _enigma.Solve.AddListener(OnEnigmaSolve);
      _ghostAnimator = GetComponent<Animator>();
   }

   private void OnEnigmaSolve() {
      _ghostAnimator.SetBool("IsLeaving", true);
      _enigma.Solve.RemoveListener(OnEnigmaSolve);
   }
}
