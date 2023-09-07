using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public static AudioManager audioManager;
    [SerializeField] private AudioSource soundEffectsAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectsAudioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public void MuteSound()
    {
        if (soundEffectsAudioSource.mute == false)
            soundEffectsAudioSource.mute = true;
        else
            soundEffectsAudioSource.mute = false;
    }

    public void MuteMusic()
    {
        if (musicAudioSource.mute == false)
            musicAudioSource.mute = true;
        else
            musicAudioSource.mute = false;
    }

    public void SetSoundEffectVolume(float volume)
    {
        soundEffectsAudioSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }
        
}
