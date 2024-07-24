using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlatformPrefab;
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
            Instantiate(PlatformPrefab, spawnPosition, Quaternion.identity);
            
            var randomInt = Random.Range(1,101);
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
