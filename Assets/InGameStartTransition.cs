using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStartTransition : MonoBehaviour
{
    public GameObject XRCameraOffset;

    private void Start()
    {
        XRCameraOffset.transform.localPosition = new Vector3(0, XRCameraOffset.transform.localPosition.y , 0);
        XRCameraOffset.transform.localRotation = Quaternion.Euler(0,90,0);
    }
}
