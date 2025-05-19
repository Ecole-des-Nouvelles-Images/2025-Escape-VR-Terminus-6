using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bipper : MonoBehaviour
{
    public Material matGreen;
    public Material matRed;
    public bool anomalyDetected;

    private Enigma _enigma;

    public UnityEvent Anomaly;
    private void FixedUpdate()
    {
        //GetComponent<Renderer>().material = anomalyDetected ? matRed : matGreen;
    }

    private void Start() {
        _enigma = GameManager.Instance.AssignEnigma();
        _enigma.Begin.AddListener(ChillBlinking);
        _enigma.Solve.AddListener(NoBlinking);
    }

    public void ChangeBool()
    {
        anomalyDetected = !anomalyDetected;
        Anomaly.Invoke();
    }

    public void ChillBlinking()
    {
        StartCoroutine(ChillBlinkingCoroutine());
    }

    public void NoBlinking()
    {
        StopAllCoroutines();
        GetComponent<MeshRenderer>().material = matGreen;
    }

    IEnumerator ChillBlinkingCoroutine()
    {
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
        StartCoroutine(ChillBlinkingCoroutine());
    }
}
