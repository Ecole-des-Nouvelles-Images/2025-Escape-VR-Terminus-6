using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    [Header("Code display")]
    public TextMeshProUGUI codeDisplay;
    private string currentCode = "___";
    [SerializeField] private string _unlockCode;
    private int currentIndex = 0;

    private bool _radioLocked;

    [Header("Engima")]
    [SerializeField] private Enigma _enigma;
    public CodeObjectDatabase codeDatabase;

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
        currentCode = "___";
        _radioLocked = true;
        UpdateCodeDisplay();
        _lockImage.sprite = _lockedSprite;
        _lockImage.color = _lockedColor;
    }

    public void AddNumber(int number)
    {
        if (currentIndex < 3)
        {
            char[] codeArray = currentCode.ToCharArray();
            codeArray[currentIndex] = number.ToString()[0];
            currentCode = new string(codeArray);
            currentIndex++;
            UpdateCodeDisplay();
        }
    }

    public void ValidateCode()
    {
        Debug.Log("Code validé : " + currentCode);
        TestCode();

        currentCode = "___";
        currentIndex = 0;
        UpdateCodeDisplay();
    }

    private void UpdateCodeDisplay() {
        codeDisplay.text = currentCode;
    }

    private void TestCode()
    {
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
            if (obj != null) {
                if (pair.code == currentCode && GameManager.Instance.currentEnigma == 1) {
                    _enigma.Solve.Invoke();
                    if (obj.GetComponent<AudioCode>()) { obj.GetComponent<AudioCode>().ActivateCode(currentCode); } 
                }
            }
        }
    }
}