using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bipper : MonoBehaviour
{
    public Material matGreen;
    public Material matRed;
    public bool anomalyDetected;

    private void FixedUpdate()
    {
        //GetComponent<Renderer>().material = anomalyDetected ? matRed : matGreen;
    }

    public void ChangeBool()
    {
        anomalyDetected = !anomalyDetected;
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
