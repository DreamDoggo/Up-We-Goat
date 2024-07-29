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

    void Update()
    {
        if (LevelManager.Level == 3){
            birdTimer += Time.deltaTime;
            if (birdTimer >= birdSpawnTime){
                float random = Random.Range(0, maxBirdHeight);
                Instantiate(bird, new Vector2(-9, player.transform.position.y + random), Quaternion.identity);
                birdTimer = 0;

            }
        }
    }
}
