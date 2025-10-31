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
    public XRButton _doorButton;

    [Header("Lock Display")]
    [SerializeField] private Image _lockImage;
    [SerializeField] private Sprite _unlockedSprite;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Color _unlockedColor;
    [SerializeField] private Color _lockedColor;

    [Header("Sound & Immersion")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _radioAmbiance;
    //public bool RadioLocked;
    //public MeshRenderer LockIndicator;
    //public Material LockOnMaterial;
    //public Material LockOffMaterial;

    private bool _isShowingMessage;
    
    void Start() {
        ChangeStation.AddListener(OnChageStation);
        _radioLocked = true;
        //UpdateCodeDisplay();
        _lockImage.sprite = _lockedSprite;
        _lockImage.color = _lockedColor;
        _audioSource.clip = _errorSound;
    }

    public void AddNumber(int number)
    {
        _isShowingMessage = false;
        /*if (currentIndex == 0)
        {
            codeDisplay.text = "";
        }*/
        
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
        codeDisplay.text = (TestCode() ? "OK" : "ERR");
        currentIndex = 0;
        _isShowingMessage = true;
        currentCode = "___";
    }

    private void UpdateCodeDisplay() {
        if (_isShowingMessage) { return; }
        
        codeDisplay.text = currentCode;
    }

    private void OnChageStation() {
        station = GameManager.Instance.currentStation;
        _enigma = GameManager.Instance.AssignEnigma();
    }

    private bool TestCode() {
        if (_radioLocked && currentCode == _unlockCode) {
            _radioLocked = false;
            Debug.Log("Radio Unlocked");
            _audioSource.clip = _radioAmbiance;
            _audioSource.Play();
            _lockImage.sprite = _unlockedSprite;
            _lockImage.color = _unlockedColor;
            return true;
        } else if (_radioLocked && currentCode != _unlockCode) {
                Debug.Log("Wrong code");
                return false;
        }
        
        if (_radioLocked) return false;

        foreach (var pair in codeDatabase.codeObjects) {
            GameObject obj = GameObject.Find(pair.objectName);
            if (obj == null) return false;

            if (pair.code == currentCode && currentCode == station.code) {
                Debug.Log("Reporting issue");
                station.Report.Invoke();
                Debug.Log("Issue reported");

                if (station.id == 1) {
                    _enigma.Solve.Invoke();
                    return true;
                }

                if (GameManager.Instance.currentEnigma == 3)
                {
                    _doorButton.GetComponent<BoxCollider>().enabled = true;
                    _enigma.Solve.Invoke();
                    return true;
                }
            }
        }
        return false;
    }
}