using UnityEngine;

public class Ghost : MonoBehaviour {
   [SerializeField] private Animator _ghostAnimator;
   [SerializeField] private Enigma _enigma;
   
   private void Start() {
      
      _enigma.Solve.AddListener(OnEnigmaSolve);
      _ghostAnimator = GetComponent<Animator>();
   }

   private void OnEnigmaSolve() {
      _ghostAnimator.SetBool("IsLeaving", true);
      _enigma.Solve.RemoveListener(OnEnigmaSolve);
   }
}
