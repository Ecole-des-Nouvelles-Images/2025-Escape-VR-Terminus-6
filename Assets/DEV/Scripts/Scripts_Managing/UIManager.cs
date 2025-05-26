using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;

    private void Start(){
	_settingsMenu.SetActive(false);
    }
    
    public void OpenSettings(){
	_mainMenu.SetActive(false);
	_settingsMenu.SetActive(true);
    }

    public void ReturnToMain(){
	_settingsMenu.SetActive(false);
	_mainMenu.SetActive(true);
    }
}
