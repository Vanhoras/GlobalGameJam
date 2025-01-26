using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    public static MusicBox Instance { get; private set; }
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    
    private float originalVolumeLevel;
    private float duckVolumeLevelPercentage = 0.2f;

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this);
        } 
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalVolumeLevel = audioSource.volume;
    }

    public void SwitchToMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    public void SwitchToGameplayMusic()
    {
        audioSource.clip = gameplayMusic;
        audioSource.Play();
    }

    public void DuckVolume()
    {
        audioSource.volume = duckVolumeLevelPercentage * originalVolumeLevel;
        Invoke(nameof(RestoreVolume), 0.8f);
    }
    
    public void RestoreVolume()
    {
        audioSource.volume = originalVolumeLevel;
    }
}
