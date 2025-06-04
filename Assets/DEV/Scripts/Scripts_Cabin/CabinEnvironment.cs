using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CabinEnvironment : MonoBehaviour {
    [SerializeField] private GameObject _back;
    [SerializeField] private Enigma _enigma;

    [SerializeField] private List<GameObject> _firstCabinContents;
    [SerializeField] private List<GameObject> _secondCabinContents;
    
    [SerializeField] private Portal _switchPortal;
    // Start is called before the first frame update
    void Start() {
        SetupEventListeners();
    }

    private void SetupEventListeners() {
        _enigma.Solve.AddListener(OnEnigmaSolved);
        //PortalCrossed.AddListener(ChangeCabin);
//        _switchPortal.CabinSwitch.AddListener(ChangeCabin);
    }

    private void OnEnigmaSolved() {
        _back.SetActive(false);
    }

    private void ChangeCabin() {
        foreach (GameObject goa in _firstCabinContents) {
            goa.SetActive(false);
        }

        foreach (GameObject gob in _secondCabinContents) {
            gob.SetActive(true);
        }
    }
    
}
