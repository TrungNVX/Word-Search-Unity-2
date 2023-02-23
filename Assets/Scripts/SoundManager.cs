using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool muteBgMusic;
    private bool muteSoundFx;
    public static SoundManager Instance;

    private AudioSource audioSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    public void ToggleBgMusic()
    {
        this.muteBgMusic = !this.muteBgMusic;
        if(this.muteBgMusic )
        {
            this.audioSource.Stop();
        }
        else
        {
            this.audioSource.Play();
        }
    }
    public void ToggleSoundFxMusic()
    {
        this.muteSoundFx = !this.muteSoundFx;
        GameEvents.ToggleSoundFxMethod();
    }
    public bool IsBgMusicMuted()
    {
        return this.muteBgMusic;
    }
    public bool IsSoundFxMuted()
    {
        return this.muteSoundFx;
    }
    public void SilenceBgMusic(bool silence)
    {
        if(!this.muteBgMusic)
        {
            if(silence)
                this.audioSource.volume = 0f;
            else
                this.audioSource.volume = 1f;
        }
    }
}

