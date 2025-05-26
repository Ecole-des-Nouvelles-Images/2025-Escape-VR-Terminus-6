using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindnessBoolChanger : MonoBehaviour
{ 
    [SerializeField] private InWallBlindness inWallBlindness;

    private void Awake()
    {
        inWallBlindness = GetComponentInParent<InWallBlindness>();
    }

    public void DarkTrue()
    {
        inWallBlindness.Dark = true;
    }

    public void DarkFalse()
    {
        inWallBlindness.Dark = false;
    }
}
