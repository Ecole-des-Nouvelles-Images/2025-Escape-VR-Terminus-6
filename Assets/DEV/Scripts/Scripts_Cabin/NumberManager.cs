using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    [Header("Code display")]
    public TextMeshProUGUI codeDisplay;
    [SerializeField] private string _unlockCode;
    private string currentCode = "___";
    private int currentIndex = 0;
    private bool _radioLocked;

    [Header("Engima")]
    public Station station;
    public UnityEvent ChangeStation;
    public CodeObjectDatabase codeDatabase;
    [SerializeField] private Enigma _enigma;

    [Header("Lock Display")]
    [SerializeField] private Image _lockImage;
    [SerializeField] private Sprite _unlockedSprite;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Color _unlockedColor;
    [SerializeField] private Color _lockedColor;
    //public bool RadioLocked;
    //public MeshRenderer LockIndicator;
    //public Material LockOnMaterial;
    //public Material LockOffMaterial;
    
    void Start() {
        ChangeStation.AddListener(OnChageStation);
        currentCode = "___";
        _radioLocked = true;
        UpdateCodeDisplay();
        _lockImage.sprite = _lockedSprite;
        _lockImage.color = _lockedColor;
    }

    public void AddNumber(int number) {
        if (currentIndex < 3) {
            char[] codeArray = currentCode.ToCharArray();
            codeArray[currentIndex] = number.ToString()[0];
            currentCode = new string(codeArray);
            currentIndex++;
            UpdateCodeDisplay();
        }
    }

    public void ValidateCode() {
        Debug.Log("Code validé : " + currentCode);
        TestCode();

        currentCode = "___";
        currentIndex = 0;
        UpdateCodeDisplay();
    }

    private void UpdateCodeDisplay() {
        codeDisplay.text = currentCode;
    }

    private void OnChageStation() {
        station = GameManager.Instance.currentStation;
        _enigma = GameManager.Instance.AssignEnigma();
    }

    private void TestCode() {
        if (_radioLocked && currentCode == _unlockCode) {
            _radioLocked = false;
            Debug.Log("Radio Unlocked");
            _lockImage.sprite = _unlockedSprite;
            _lockImage.color = _unlockedColor;
        } else if (_radioLocked && currentCode != _unlockCode) {
                currentCode = "Err";
                UpdateCodeDisplay();
                Debug.Log("Wrong code");
        }
        
        if (_radioLocked) return;
        
        foreach (var pair in codeDatabase.codeObjects) {
            GameObject obj = GameObject.Find(pair.objectName);
            if (obj == null) return; 
            
            if (pair.code == currentCode && currentCode == station.code) { //Need a code for every enigma
                Debug.Log("Reporting issue");
                station.Report.Invoke();
                Debug.Log("Issue reported");
                    
                if (GameManager.Instance.currentEnigma == 1 || GameManager.Instance.currentEnigma == 3) {
                    _enigma.Solve.Invoke();
                }

                if (obj.GetComponent<AudioCode>()) { obj.GetComponent<AudioCode>().ActivateCode(currentCode); } 
            }
        }
    }
}