using CharlieMadeAThing.TooManyBricks.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

namespace CharlieMadeAThing.TicTicTacTacToeToe {
    public class SoundManager : Singleton<SoundManager> {
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] AudioSource sfxAudioSource;
        [SerializeField] AudioSource musicAudioSource;
        [SerializeField] AudioSource emitterPrefab;
        
        public void PlayOneShot( AudioClip clip, float volume ) {
            if ( !clip ) {
                Debug.LogError($"[SoundManager] No clip provided.");
                return;
            }
            sfxAudioSource.clip = clip;
            sfxAudioSource.volume = volume;
            sfxAudioSource.Play();
        }
        
        public void PlayOneShotAt( AudioClip clip, float volume, Transform location ) {
            if ( emitterPrefab == null ) {
                Debug.LogError($"[SoundManager] No emitter prefab set.");
                return;
            }
            if ( !clip ) {
                Debug.LogError($"[SoundManager] No clip provided.");
                return;
            }

            var emitter = Instantiate(emitterPrefab, location.position, Quaternion.identity);
            emitter.clip = clip;
            emitter.volume = volume;
            emitter.Play();
            Destroy(emitter.gameObject, clip.length);
        }

        public void PlayMusic( AudioClip clip, float volume, bool loop ) {
            if ( !clip ) {
                Debug.LogError($"[SoundManager] No clip provided.");
                return;
            }
            musicAudioSource.clip = clip;
            musicAudioSource.volume = volume;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
        }
        
    
        public void SetMasterVolume( float level ) {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        }
    
        public void SetSoundFxVolume( float level ) {
            audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
        }
        
        public void SetMusicVolume( float level ) {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        }
    }
}
