using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour {

    public AudioSource audioSource;
    public enum SoundType { SFX,Music,Announcer};
    public SoundType soundType;

    float originVol;
    void Start () {
        originVol = audioSource.volume;
        SetupSound();
    }

    public void SetupSound()
    {
        float currentVol = originVol;

        if (soundType == SoundType.SFX)
        {
            float pref = PlayerPrefsConstants.GetSFXVolume();
            audioSource.volume = currentVol / 100 * (pref * 100);
        }
        else if (soundType == SoundType.Music)
        {
            float pref = PlayerPrefsConstants.GetMusicVolume();
            audioSource.volume = currentVol / 100 * (pref * 100);
        }
        else if (soundType == SoundType.Announcer)
        {
            float pref = PlayerPrefsConstants.GetAnnouncerVolume();
            audioSource.volume = currentVol / 100 * (pref * 100);
        }
    }

}
