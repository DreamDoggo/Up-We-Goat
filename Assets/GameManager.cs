using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour
{
    [SerializeField] public LevelManager levels;
    public GameObject[] PlatformPrefabs = new GameObject[2];
    //public Transform[] PlatformSpawnLocations = new Transform[3];
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
            if (LevelManager.Level == 1){
                var randomPlatform = Random.Range(0, 3);
                if (randomPlatform >= 1){
                    Instantiate(PlatformPrefabs[1], spawnPosition, Quaternion.identity);
                } else {
                    Instantiate(PlatformPrefabs[0], spawnPosition, Quaternion.identity);
                }
            }
            Instantiate(PlatformPrefabs[0], spawnPosition, Quaternion.identity);
            var randomInt = Random.Range(1,101);
            if (randomInt <= CollectableSpawnChance)
            {
                Instantiate(CollectablePrefab, new Vector2(spawnPosition.x,spawnPosition.y+coinDistance), Quaternion.identity);
            }
        }
    }

    private void PlaceNewLevel() 
    {
        switch (LevelManager.Level) 
        {
            case 0:
                return;
            case 1:
                return;
            case 2:
                return;
            default:
                return;
        }
    }

    private void PlacePlatforms(Transform spawnPosition, GameObject[] platformPrefabs) 
    {
        
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
