using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberManager : MonoBehaviour
{
    public TextMeshProUGUI codeDisplay;
    private string currentCode = "___";
    private int currentIndex = 0;

    public CodeObjectDatabase codeDatabase;
    //public bool RadioLocked;
    //public MeshRenderer LockIndicator;
    //public Material LockOnMaterial;
    //public Material LockOffMaterial;
    
    void Start()
    {
        currentCode = "___";
        UpdateCodeDisplay();
    }

    /*private void Update()
    {
        if (RadioLocked)
        {
            if (LockIndicator.material != LockOnMaterial)
            {
                LockIndicator.material = LockOnMaterial;
            }
        }
        else
        {
            if (LockIndicator.material != LockOffMaterial)
            {
                LockIndicator.material = LockOffMaterial;
            }
        }
    }*/

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
        /*if (RadioLocked)
        {
            if (currentCode == "486")
            {
                RadioLocked = false;
            }
        }
        if (RadioLocked) return;*/
        foreach (var pair in codeDatabase.codeObjects) {
            GameObject obj = GameObject.Find(pair.objectName);
            if (obj != null) {
                if (pair.code == currentCode) {
                    if (obj.GetComponent<AudioCode>()) { obj.GetComponent<AudioCode>().ActivateCode(currentCode); } 
                }
            }
        }
    }
}