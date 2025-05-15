using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    
    public Camera ViewCamera;  
    public List<Transform> CameraTransforms = new List<Transform>();

    private int _currentCameraIndex = 0;

    private void Awake() {
        UpdateCamera();
    }
    
    [ContextMenu("Increment Camera")]
    public void IncrementCameraIndex() {
        _currentCameraIndex += 1;
        if (_currentCameraIndex >= CameraTransforms.Count) { _currentCameraIndex = 0; }
        UpdateCamera();
    }
    [ContextMenu("Decrement Camera")]
    public void DecrementCameraIndex() {
        _currentCameraIndex -= 1;
        if (_currentCameraIndex < 0) { _currentCameraIndex = CameraTransforms.Count - 1; }
        UpdateCamera();
    }
    private void UpdateCamera() {
        ViewCamera.transform.position = CameraTransforms[_currentCameraIndex].position;
        ViewCamera.transform.rotation = CameraTransforms[_currentCameraIndex].rotation;
    }

    
}
