using System.Collections;
using System.Collections.Generic;
using CharlieMadeAThing.TooManyBricks.Core;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager> {
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource sfxAudioSource;
    public void PlaySoundFx( AudioClip clip, float volume ) {
        sfxAudioSource.clip = clip;
        sfxAudioSource.volume = volume;
        sfxAudioSource.Play();
    }
    
    public void SetMasterVolume( float level ) {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }
    
    public void SetSoundFxVolume( float level ) {
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }
}
