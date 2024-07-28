using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour
{
    [SerializeField] public LevelManager levels;
    [SerializeField] GameObject[] PlatformPrefabs = new GameObject[2];
    [SerializeField] Transform[] PlatformSpawnLocations = new Transform[3];
    /*
    PlatformPrefab[0] = Basic Platform
    PlatformPrefab[1] = Ice Platform
    */

    [Header("Platform Settings For Each Level " +
        "\nElements line up with each other" +
        "\nTotal spawn chances should add to 100")]
    [SerializeField] GameObject[] LevelOnePlatforms = new GameObject[1];
    [Range(0, 100)]
    [SerializeField] int[] LevelOnePlatformChances = new int[1];
    [Space(5)]
    [SerializeField] GameObject[] LevelTwoPlatforms = new GameObject[2];
    [Range(0, 100)]
    [SerializeField] int[] LevelTwoPlatformChances = new int[2];
    [Space(5)]
    [SerializeField] GameObject[] LevelThreePlatforms = new GameObject[1];
    [Range(0, 100)]
    [SerializeField] int[] LevelThreePlatformChances = new int[1];
    [Space(5)]
    [SerializeField] GameObject[] LevelFourPlatforms = new GameObject[2];
    [Range(0, 100)]
    [SerializeField] int[] LevelFourPlatformChances = new int[2];

    
    public GameObject CollectablePrefab;
    [SerializeField] int PlatformCount = 300;
    [SerializeField] float CollectableSpawnDistance = 1.5f;
    [SerializeField] float PlatformSpawnHeight = 1.75f;

    [Tooltip("What is the chance out of 100 that a collectable will spawn on any given platform")]
    [Range(1, 100)]
    [SerializeField] int CollectableSpawnChance = 33;


    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnPosition = new Vector2(0f, -8f);

        /*for (int i = 0; i < PlatformCount; i++)
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
        }*/
    }

    /// <summary>
    /// Determines what level layout to set up depending on what level the Player just entered
    /// </summary>
    public void PlaceNewLevel() 
    {
        switch (LevelManager.Level) 
        {
            case 1:
                PlacePlatformsFinite(PlatformSpawnLocations[0].position, PlatformSpawnLocations[1].position.y, LevelOnePlatforms, LevelOnePlatformChances);
                break;
            case 2:
                PlacePlatformsFinite(PlatformSpawnLocations[1].position, PlatformSpawnLocations[2].position.y, LevelTwoPlatforms, LevelTwoPlatformChances);
                break;
            case 3:
                PlacePlatformsFinite(PlatformSpawnLocations[2].position, PlatformSpawnLocations[3].position.y, LevelThreePlatforms, LevelThreePlatformChances);
                break;
            default:
                break;
        }
        return;
    }

    /// <summary>
    /// Handles the logic related to placing platforms below a known height
    /// </summary>
    /// <param name="spawnPosition">
    /// The position where the platforms will start spawning from
    /// </param>
    /// <param name="platformPrefabs">
    /// The prefabs for the platforms that will be spawned
    /// </param>
    /// <param name="placeUntilHeight">
    /// The height at which to stop placing platforms
    /// </param>
    /// <param name="platformChances">
    /// The chance (0-100) that a given platform will be placed instead of another.
    /// Index 0 = default, Index 1 = icy
    /// </param>
    private void PlacePlatformsFinite(Vector2 spawnPosition, float placeUntilHeight, GameObject[] platformPrefabs, int[] platformChances) 
    {
        // Keep spawning platforms until we hit the next stage
        while (spawnPosition.y <= placeUntilHeight) 
        {
            spawnPosition.y += PlatformSpawnHeight;
            spawnPosition.x = Random.Range(-5f, 5f);

            // Spawn a platform based on how common it is
            int randomInt = Random.Range(1, 101);
            
            for (int i = 0; i < platformPrefabs.Length; i++)
            {
                if (randomInt <= platformChances[i]) 
                {
                    GameObject platformSpawned = Instantiate(platformPrefabs[i], spawnPosition, Quaternion.identity);
                    PlaceCollectable(CollectablePrefab, platformSpawned);
                    break;
                }
            }  
        }
        return;
    }

    private void PlaceCollectable(GameObject collectablePrefab, GameObject platformToSpawnOn) 
    {
        int randomInt = Random.Range(1, 101);
        if (randomInt <= CollectableSpawnChance)
        {
            Instantiate(collectablePrefab, new Vector2(platformToSpawnOn.transform.position.x, platformToSpawnOn.transform.position.y + CollectableSpawnDistance), Quaternion.identity);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
