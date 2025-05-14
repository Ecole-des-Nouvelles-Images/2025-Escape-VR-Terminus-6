using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public TextMeshProUGUI musicButtonTMP;
    private bool musicMuted;
    private float musicTempVolume;
    public TextMeshProUGUI sfxButtonTMP;
    private bool sfxMuted;
    private float sfxTempVolume;

    private void Start()
    {
        MuteMusic();
        MuteSfx();
        MuteMusic();
        MuteSfx();
    }

    public void SetMusicVolume(float sliderValue)
    {
        mainMixer.SetFloat("Volume Music", Mathf.Log10(sliderValue) * 20);
        musicTempVolume = Mathf.Log10(sliderValue) * 20;
    }
    public void SetSfxVolume(float sliderValue)
    {
        mainMixer.SetFloat("Volume SFX", Mathf.Log10(sliderValue) * 20);
        sfxTempVolume = Mathf.Log10(sliderValue) * 20;
    }
    
    [ContextMenu("Toggle Music")]
    public void MuteMusic() {
        if (musicMuted) {
            UnmuteMusic();
        }
        else {
            mainMixer.SetFloat("Volume Music", Mathf.Log10(0) * 20);
            musicSlider.value = musicSlider.minValue;
            musicMuted = true;
            musicButtonTMP.text = "Music : OFF";
        }
    }
    private void UnmuteMusic() {
        musicSlider.value = 1;
        mainMixer.SetFloat("Volume Music", musicTempVolume);
        musicMuted = false;
        musicButtonTMP.text = "Music : ON";
    }
    
    [ContextMenu("Toggle SFX")]
    public void MuteSfx()
    {
        if (sfxMuted) {
            UnmuteSfx();
        }
        else {
            mainMixer.SetFloat("Volume SFX", Mathf.Log10(0) * 20);
            sfxSlider.value = sfxSlider.minValue;
            sfxMuted = true;
            sfxButtonTMP.text = "SFX : OFF";
        }
    }

    private void UnmuteSfx()
    {
        sfxSlider.value = 1;
        mainMixer.SetFloat("Volume SFX", sfxTempVolume);
        sfxMuted = false;
        sfxButtonTMP.text = "SFX : ON";
    }
}
