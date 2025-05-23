using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWallBlindness : MonoBehaviour
{
    [SerializeField] Animator BwcAnimator;
    public bool Dark;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Environnement") && Dark == false)
        {
            BwcAnimator.SetBool("InWall", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Environnement") && Dark)
        {
            BwcAnimator.SetBool("InWall", false);
        }
    }

    
}
