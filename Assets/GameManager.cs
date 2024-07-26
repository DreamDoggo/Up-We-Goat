using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public levelManager levels;
    public GameObject[] PlatformPrefab = new GameObject[2];
    /*
    PlatformPrefab[0] = Basic Platform
    PlatformPrefab[1] = Ice Platform
    */
    
    public GameObject CollectablePrefab;
    [SerializeField] int PlatformCount = 300;
    [SerializeField] float coinDistance = 1.5f;
    [SerializeField] float PlatformSpawnHeight = 1.75f;

    [Tooltip("What is the chance out of 100 that a collectable will spawn on any given platform")]
    [Range(1, 100)]
    [SerializeField] int CollectableSpawnChance = 33;


    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnPosition = new Vector2(0f, -8f);

        for (int i = 0; i < PlatformCount; i++)
        {
            spawnPosition.y += PlatformSpawnHeight;
            spawnPosition.x = Random.Range(-5f, 5f);
            if (levelManager.level == 1){
                var randomPlatform = Random.Range(0, 3);
                if (randomPlatform >= 1){
                    Instantiate(PlatformPrefab[1], spawnPosition, Quaternion.identity);
                } else {
                    Instantiate(PlatformPrefab[0], spawnPosition, Quaternion.identity);
                }
            }
            Instantiate(PlatformPrefab[0], spawnPosition, Quaternion.identity);
            var randomInt = Random.Range(1,3);
            if (randomInt <= CollectableSpawnChance)
            {
                Instantiate(CollectablePrefab, new Vector2(spawnPosition.x,spawnPosition.y+coinDistance), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
