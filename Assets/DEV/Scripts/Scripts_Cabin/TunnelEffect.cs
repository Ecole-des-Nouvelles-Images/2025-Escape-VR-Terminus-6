using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelEffect : MonoBehaviour
{
    public Tunnel tunnel;
    public AudioSource audioSource;


    private float _vol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _vol = tunnel.GetSpeed();
        audioSource.volume = (_vol / tunnel.GetAcceleration());
    }
}
