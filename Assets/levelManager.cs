using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The level which the player is currently in")]
    public static int Level = 1;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        Level = 1;
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Player")
        {
            if (Level == 3)
            {
                transform.position = new Vector2(0, gameManager.NextInfiniteSpawnPosition.y - 5f);
                MusicManager musicManager = FindFirstObjectByType<MusicManager>();
                if (musicManager != null) 
                {
                    musicManager.Crossfade(musicManager.SpaceMusic, musicManager.SpaceMusicVolume);
                }
            }
            if (Level != 4)
            {
                Level++;
            }
            gameManager.PlaceNewLevel();
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
            transform.position = new Vector2(0, gameManager.NextInfiniteSpawnPosition.y - 5f);
            gameManager.PlaceNewLevel();
        }
    }
}
