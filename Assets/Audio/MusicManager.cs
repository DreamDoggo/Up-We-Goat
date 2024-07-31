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
    [SerializeField] float TitleMusicVolume = 1f;
    [Range(0, 1)]
    [SerializeField] float GameMusicVolume = .75f;
    [Range(0, 1)]
    [SerializeField] float GameOverMusicVolume = .75f;

    [Space(10)]
    [SerializeField] AudioClip TitleMusic, GameMusic, GameOverMusic;
    [Space(10)]
    [SerializeField] AudioSource RefAudioSource1;
    [SerializeField] AudioSource RefAudioSource2;
    bool MusicPlaying = false;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchMusic(RefAudioSource1, TitleMusic);
        StartCoroutine(StartFade(RefAudioSource1, 2f, 1f));
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level) 
        {
            case 0:
                Crossfade(TitleMusic, TitleMusicVolume);
                break;
            case 1:
                Crossfade(GameMusic, GameMusicVolume);
                break;
            case 2:
                StartCoroutine(StartFade(FindCurrentlyPlayingAudioSource(), 2f, 0f));
                MusicPlaying = false;
                break;
            case 3:
                Crossfade(GameOverMusic, GameOverMusicVolume);
                new WaitForSeconds(CrossfadeDuration);
                FindCurrentlyPlayingAudioSource().loop = false;
                break;
            default:
                StartCoroutine(StartFade(FindCurrentlyPlayingAudioSource(), 2f, 0f));
                MusicPlaying = false;
                break;
        }
    }

    public void Crossfade(AudioClip newMusic, float targetCrossfadeInVolume) 
    {
        AudioSource currentAudioSource = FindCurrentlyPlayingAudioSource();
        if (MusicPlaying) 
        {
            StartCoroutine(StartFade(currentAudioSource, CrossfadeDuration, TargetCrossfadeOutVolume));
            AudioSource newAudioSource = FindFreeAudioSource();
            newAudioSource.loop = true;
            SwitchMusic(newAudioSource, newMusic);
            StartCoroutine(StartFade(newAudioSource, CrossfadeDuration, targetCrossfadeInVolume));
        }
        else if (!MusicPlaying) 
        {
            SwitchMusic(currentAudioSource, newMusic);
            StartFade(RefAudioSource1, CrossfadeDuration, targetCrossfadeInVolume);
        }
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