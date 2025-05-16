using UnityEngine;
using UnityEngine.Events;

public class CabinEnvironment : MonoBehaviour {
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _portal;
    [SerializeField] private Enigma _enigma;

    [SerializeField] private Portal _switchPortal;
    // Start is called before the first frame update
    void Start() {
        _portal.SetActive(false);
        SetupEventListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupEventListeners() {
        _enigma.Solve.AddListener(OnEnigmaSolved);
        //PortalCrossed.AddListener(ChangeCabin);
        _switchPortal.CabinSwitch.AddListener(ChangeCabin);
    }

    private void OnEnigmaSolved() {
        _back.SetActive(false);
        _portal.SetActive(true);
    }

    private void ChangeCabin() {
        for (int i = 0; i < this.transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
}
