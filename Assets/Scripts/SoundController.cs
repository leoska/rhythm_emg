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
    
    [Header("Music")]
    public AudioSource music = null;
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
    }

    public void SuccessNote()
    {
        if (Math.Abs(musicFilter.cutoffFrequency - SuccessFrequency) > 0.01f)
            musicFilter.cutoffFrequency = SuccessFrequency;
    }
}
