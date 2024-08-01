using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using System.Runtime.CompilerServices;

public class MusicManager : MonoBehaviour
{
    [SerializeField] float CrossfadeDuration = 1.5f;
    float TargetCrossfadeOutVolume = 0f;
    [Range(0, 1)]
    [SerializeField] public float TitleMusicVolume = 1f;
    [Range(0, 1)]
    [SerializeField] public float GameMusicVolume = .75f;
    [Range(0, 1)]
    [SerializeField] public float GameOverMusicVolume = .75f;
    [Range(0, 1)]
    [SerializeField] public float SpaceMusicVolume = 1f;

    [Space(10)]
    [SerializeField] public AudioClip TitleMusic, GameMusic, GameOverMusic, SpaceMusic;
    [Space(10)]
    [SerializeField] AudioSource RefAudioSource1;
    [SerializeField] AudioSource RefAudioSource2;
    bool MusicPlaying = false;
    bool ComingFromGameOver = false;
    bool InSpace = false;
    int conMusic = 0;
    

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SwitchMusic(RefAudioSource1, TitleMusic);
        StartCoroutine(StartFade(RefAudioSource1, 2f, 1f));
    }

    private void OnLevelWasLoaded(int level)
    {
        if (gameObject != null) 
        {
            StopAllCoroutines();
            EnableLooping();
            
            switch (level) 
            {
                // Main Menu
                case 0:
                    if (ComingFromGameOver)
                    {
                        ClearClips();
                        SwitchMusic(RefAudioSource1, TitleMusic);
                        StartCoroutine(StartFade(RefAudioSource1, CrossfadeDuration, TitleMusicVolume));
                        ComingFromGameOver = false;
                        break;
                    }
                    //Crossfade(TitleMusic, TitleMusicVolume);
                    break;

                // Gameplay
                case 1:
                    if (ComingFromGameOver)
                    {
                        ClearClips();
                        SwitchMusic(RefAudioSource1, GameMusic);
                        StartCoroutine(StartFade(RefAudioSource1, CrossfadeDuration, GameMusicVolume));
                        ComingFromGameOver = false;
                        break;
                    }
                    Crossfade(GameMusic, GameMusicVolume);
                    break;

                // Tips
                case 2:
                    conMusic = 1;
                    break;

                // Game Over
                case 3:
                    Crossfade(GameOverMusic, GameOverMusicVolume);
                    DisableLooping();
                    ComingFromGameOver = true;
                    break;
                    
                // I assume credits
                case 4:
                    conMusic = 1;
                    break;
                default:
                    StartCoroutine(StartFade(FindCurrentlyPlayingAudioSource(), 2f, 0f));
                    MusicPlaying = false;
                    break;
            }
        }
    }

    public void Crossfade(AudioClip newMusic, float targetCrossfadeInVolume) 
    {
        AudioSource currentAudioSource = FindCurrentlyPlayingAudioSource();
        if (MusicPlaying) 
        {
            AudioSource newAudioSource = FindFreeAudioSource();
            SwitchMusic(newAudioSource, newMusic);
            StartCoroutine(StartFade(currentAudioSource, CrossfadeDuration, TargetCrossfadeOutVolume));
            StartCoroutine(StartFade(newAudioSource, CrossfadeDuration, targetCrossfadeInVolume));
        }
        if (MusicPlaying == false) 
        {
            ClearClips();
            SwitchMusic(RefAudioSource1, newMusic);
            StartFade(RefAudioSource1, CrossfadeDuration, targetCrossfadeInVolume);
            EnableLooping();
        }
    }

    public void ToggleLooping() 
    {
        RefAudioSource1.loop = !RefAudioSource1.loop;
        RefAudioSource2.loop = !RefAudioSource2.loop;
    }

    public void EnableLooping() 
    {
        RefAudioSource1.loop = true;
        RefAudioSource2.loop = true;
    }

    public void DisableLooping() 
    {
        RefAudioSource1.loop = false;
        RefAudioSource2.loop = false;
    }

    public void ClearClips() 
    {
        RefAudioSource1.clip = null;
        RefAudioSource2.clip = null;
    }

    public void SwitchMusic(AudioSource audioSource, AudioClip newClip) 
    {
        audioSource.clip = newClip;
        audioSource.volume = 0;
        audioSource.Play();
        MusicPlaying = true;
    }

    public AudioSource FindCurrentlyPlayingAudioSource() 
    {
        if (RefAudioSource1.isPlaying) 
        {
            MusicPlaying = true;
            return RefAudioSource1;
        }
        else if (RefAudioSource2.isPlaying)
        {
            MusicPlaying = true;
            return RefAudioSource2;
        }
        else 
        {
            MusicPlaying = false;
            return RefAudioSource1;
        }
    }

    public AudioSource FindFreeAudioSource() 
    {
        if (RefAudioSource1.isPlaying)
        {
            return RefAudioSource2;
        }
        else if (RefAudioSource2.isPlaying)
        {
            return RefAudioSource1;
        }
        else 
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.loop = true;
            newAudioSource.enabled = true;
            return newAudioSource;
        }
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if (audioSource.volume <= .1f) 
        {
            audioSource.Stop();
        }
        yield break;
    }
}
