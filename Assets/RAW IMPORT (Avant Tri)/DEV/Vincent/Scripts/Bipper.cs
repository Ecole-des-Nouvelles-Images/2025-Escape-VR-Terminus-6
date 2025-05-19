using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bipper : MonoBehaviour
{
    public Material matGreen;
    public Material matRed;
    public bool anomalyDetected;

    public Enigma enigma;

    private float _shortTimer = 0.3f;
    private float _medTimer = 0.3f;
    private float _longTimer = 0.5f;

    private float _blinkt;
    private bool _colorSelect;
    private bool _isBlinking;
    
    public UnityEvent Anomaly;
    private void Update()
    {
        if (_isBlinking) {
            ChillBlinking();
        } else {
            return;
        }
    }

    private void Start() {
        Anomaly.AddListener(BlinkSetup);
        _blinkt = 0;
        _isBlinking = false;
    }

    private void BlinkSetup() {
        enigma.Solve.AddListener(NoBlinking);
        _isBlinking = true;
    }

    public void ChillBlinking() {
        _isBlinking = true;
        if (_blinkt < _shortTimer) {
            _blinkt += Time.deltaTime;
            return;
        } else {
            _blinkt = 0f;
            _colorSelect = !_colorSelect;
            GetComponent<Renderer>().material = _colorSelect ? matRed : matGreen;
            Debug.Log("Timer Ended");
        }
    }

    public void NoBlinking() {
        _isBlinking = false;
        GetComponent<MeshRenderer>().material = matGreen;
    }

    private void ChillBlinkingCoroutine()
    {
        /*GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().material = matRed;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material = matGreen;
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChillBlinkingCoroutine());*/
    }
}
