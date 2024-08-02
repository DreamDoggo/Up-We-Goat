using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class randomFallingObstacles : MonoBehaviour
{
    [SerializeField] GameObject[] Obstacle = new GameObject[2];
    LevelManager levelman;
    [SerializeField] GameObject Player;
    [Space(10)]
    [Header("Rock/Snowball Settings")]
    [Range(-10f, 10f)]
    [SerializeField] float RockMinX;
    [Range(-10f, 10f)]
    [SerializeField] float RockMaxX;
    [Range(0f, 10f)]
    [SerializeField] float RockDistanceY;
    [Range(0f, 10f)]
    [SerializeField] float DropTimer = 10;
    [SerializeField] float timer;
    [SerializeField] AudioClip RockFallingSFX;
    [Space(10)]
    [Header("Meteor Settings\nMin must be lower than the max")]
    [Range(0f, 10f)]
    [SerializeField] float MeteorMinDistanceFromPlayer = 3f;
    [Range(5f, 15f)]
    [SerializeField] float MeteorMaxDistanceFromPlayer = 8f;
    [Range(50f, 300f)]
    [SerializeField] float MeteorMoveSpeed = 100f;
    [SerializeField] float LeftXSpawnPosition = -8f;
    [Tooltip("Time in seconds between meteor spawns")]
    [SerializeField] float MeteorTimer = 5;

    void Start(){
        levelman = FindFirstObjectByType<LevelManager>();
    }
    void Update(){
        timer = timer + Time.deltaTime;
        int level = LevelManager.Level-1;
        if (level < 3) 
        {
            SpawnRock(level);
        }
        else if (level == 3)
        {
            SpawnMeteor();
        }
    }

    void SpawnRock(int level) 
    {
        Vector3 point = new Vector3(Random.Range(RockMinX, RockMaxX), Player.transform.position.y + RockDistanceY, Player.transform.position.z);
        if (timer >= DropTimer)
        {
            try
            {
                GameObject rockspawned = Instantiate(Obstacle[level], point, Player.transform.rotation);
                AudioSource FallingAudioSource = rockspawned.GetComponent<AudioSource>();
                FallingAudioSource.clip = RockFallingSFX;
                FallingAudioSource.Play();
                timer = 0;
            }
            catch
            {
            }
        }
    }

    void SpawnMeteor() 
    {
        if (timer >= MeteorTimer) 
        {
            GameObject spawnedMeteor;
            if (Random.Range(1, 3) == 1)
            {
                spawnedMeteor = Instantiate(Obstacle[3], 
                    new Vector2(LeftXSpawnPosition, Player.transform.position.y + Random.Range(MeteorMinDistanceFromPlayer, MeteorMaxDistanceFromPlayer)),
                    Quaternion.identity);
            }
            else 
            {
                spawnedMeteor = Instantiate(Obstacle[3],
                    new Vector2(-LeftXSpawnPosition, Player.transform.position.y + Random.Range(MeteorMinDistanceFromPlayer, MeteorMaxDistanceFromPlayer)),
                    Quaternion.identity);
            }
            Meteor meteorScript = spawnedMeteor.GetComponent<Meteor>();
            meteorScript.MoveSpeed = MeteorMoveSpeed;
            timer = 0;
        }
    }
}
