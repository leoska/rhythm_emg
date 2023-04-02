using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private const float FailedFrequency = 950f;
    private const float SuccessFrequency = 22000f;
    
    [Header("Effects")] 
    public AudioSource effects = null;
    public AudioClip failed = null;
    public AudioClip menuButton = null;
    public AudioClip selectTrack = null;
    
    [Header("Music")]
    public AudioSource music = null;
    public AudioClip[] tracks = {};
    public AudioLowPassFilter musicFilter = null;

    public float GetMusicTime()
    {
        return music.time;
    }

    public float GetMusicLength()
    {
        return music.clip.length;
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void PauseMusic()
    {
        music.Pause();
    }

    public void UnPauseMusic()
    {
        music.UnPause();
    }

    public void PlayMusic()
    {
        music.Play();
    }

    public void FailedNote()
    {
        if (Math.Abs(musicFilter.cutoffFrequency - FailedFrequency) > 0.01f)
            musicFilter.cutoffFrequency = FailedFrequency;
        
        Invoke(nameof(PlayFailed), 0.3f);
    }

    public void SuccessNote()
    {
        if (Math.Abs(musicFilter.cutoffFrequency - SuccessFrequency) > 0.01f)
            musicFilter.cutoffFrequency = SuccessFrequency;
    }

    public void PlayFailed()
    {
        effects.clip = failed;
        effects.Play();
    }

    public void PlayButtonClick()
    {
        effects.clip = menuButton;
        effects.Play();
    }

    public void PlaySelectTrack()
    {
        effects.clip = selectTrack;
        effects.Play();
    }

    public void SelectTrack(int track)
    {
        music.clip = tracks[track];
    }
}
