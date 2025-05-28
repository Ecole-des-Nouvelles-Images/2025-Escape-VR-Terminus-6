using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class JoystickDisabler : MonoBehaviour
{
    public DynamicMoveProvider moveProvider;

    private void Awake()
    {
        #if UNITY_EDITOR
            moveProvider.leftHandMoveInput = null;
            moveProvider.rightHandMoveInput = null;
        #else
            throw new NotImplementedException();
        #endif
    }
}
