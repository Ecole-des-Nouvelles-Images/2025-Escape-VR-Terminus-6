using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Enigma : MonoBehaviour { 
    public UnityEvent Solve;
    public UnityEvent Begin;
    public bool isSolved;
    public int id;
}
