using System.Collections.Generic;
using UnityEngine;

public class AudioCode : MonoBehaviour {
    public AudioSource AudioSource;
    public List<AudioClipPair> AudioClips = new List<AudioClipPair>();

    public void ActivateCode(string code) {
        // Rechercher l'audio correspondant au code
        var audioClipPair = AudioClips.Find(pair => pair.Code == code);
        if (audioClipPair != null) {
            AudioSource.clip = audioClipPair.Clip;
            AudioSource.Play();
        }
        else {
            Debug.LogWarning("Aucun audio trouvé pour le code: " + code);
        }
    }

    public void StopAudio() { AudioSource.Stop(); }
}

[System.Serializable]
public class AudioClipPair {
    public string Code;
    public AudioClip Clip;
}