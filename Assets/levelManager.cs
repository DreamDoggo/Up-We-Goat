using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The level which the player is currently in")]
    public static int Level = 1;

    GameManager RefGameManager;
    [SerializeField] GameObject RefCamera;
    AudioSource RefAudioSource;
    [SerializeField] float TransitionVolume = 1.0f;
    [SerializeField] AudioClip TransitionAudio;

    private void Awake()
    {
        RefGameManager = FindFirstObjectByType<GameManager>();
        RefAudioSource = RefCamera.GetComponent<AudioSource>();
        RefAudioSource.playOnAwake = false;
        Level = 1;
    }

    private void Start()
    {
        RefAudioSource.volume = TransitionVolume;
        RefAudioSource.clip = TransitionAudio;

    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Player")
        {
            if (Level == 3)
            {
                transform.position = new Vector2(0, RefGameManager.NextInfiniteSpawnPosition.y - 5f);
                MusicManager musicManager = FindFirstObjectByType<MusicManager>();
                if (musicManager != null) 
                {
                    musicManager.Crossfade(musicManager.SpaceMusic, musicManager.SpaceMusicVolume);
                }
            }
            if (Level != 4)
            {
                Level++;
                RefAudioSource.Play();
                
            }
            RefGameManager.PlaceNewLevel();
            if (Level != 4) 
            {
                Destroy(gameObject);
            }
            HandleLevelFour();
        }
    }

    void HandleLevelFour() 
    {
        if (Level == 4) 
        {
            transform.position = new Vector2(0, RefGameManager.NextInfiniteSpawnPosition.y - 5f);
            RefGameManager.PlaceNewLevel();
        }
    }
}
