using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdSpawns : MonoBehaviour
{
    [SerializeField] GameObject bird;
    [SerializeField] GameObject player;
    [SerializeField] float maxBirdHeight;
    [SerializeField] float birdTimer;
    [SerializeField] float birdSpawnTime;
    [SerializeField] AudioClip birdSound;

    void Update()
    {
        if (LevelManager.Level == 3){
            birdTimer += Time.deltaTime;
            if (birdTimer >= birdSpawnTime){
                float random = Random.Range(0, maxBirdHeight);

                GameObject birdSpawned = Instantiate(bird, new Vector2(-9, player.transform.position.y + random), Quaternion.identity);
                AudioSource birdAudioSource = birdSpawned.GetComponent<AudioSource>();
                birdAudioSource.clip = birdSound;
                birdAudioSource.Play();
                birdTimer = 0;

            }
        }
    }
}
