using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<GameObject> Cameras = new List<GameObject>();
    private int _currentCameraIndex = 0;

    private void Awake() {
        UpdateCamera();
    }
    
    [ContextMenu("Increment Camera")]
    public void IncrementCameraIndex() {
        _currentCameraIndex += 1;
        if (_currentCameraIndex >= Cameras.Count) { _currentCameraIndex = 0; }
        UpdateCamera();
    }
    [ContextMenu("Decrement Camera")]
    public void DecrementCameraIndex() {
        _currentCameraIndex -= 1;
        if (_currentCameraIndex < 0) { _currentCameraIndex = Cameras.Count - 1; }
        UpdateCamera();
    }
    private void UpdateCamera() {
        foreach (GameObject cam in Cameras) { cam.SetActive(false); }
        Cameras[_currentCameraIndex].SetActive(true);
    }
}
