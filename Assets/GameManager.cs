using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlatformPrefab;
    [SerializeField] int PlatformCount = 300;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawnPosition = new Vector2(0f, -8f);

        for (int i = 0; i < PlatformCount; i++)
        {
            spawnPosition.y += Random.Range(1.5f, 2f);
            spawnPosition.x = Random.Range(-5f, 5f);
            Instantiate(PlatformPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
