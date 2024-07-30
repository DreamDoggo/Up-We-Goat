using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AvalancheSpawner : MonoBehaviour
{
    [SerializeField] GameObject AvalanchePrefab;
    [SerializeField] GameObject AvalancheWarningPrefab;
    [SerializeField] GameObject RefPlayer;
    [SerializeField] float AvalancheSpawnTime;
    [Range(-10f, 0f)]
    [SerializeField] float MinX = -5f;
    [Range(0f, 10f)]
    [SerializeField] float MaxX = 5f;
    [Range(8f, 15f)]
    [SerializeField] float DistanceFromPlayer = 10f;

    float AvalancheTimer;
    GameObject AvalancheSpawned;
    GameObject WarningSpawned;
    ParticleSystem RefParticles;
    SpriteRenderer RefSprite;
    bool AvalancheExists = false;
    bool ParticlesExists = false;
    bool WarningExists = false;

    private void Start()
    {
        SetRandomSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Level == 2 && !AvalancheExists) 
        {
            if (AvalancheTimer >= AvalancheSpawnTime) 
            {
                SetRandomSpawnTime();
                SpawnAvalanche();
            }
            AvalancheTimer += Time.deltaTime;
        }
        if (AvalancheExists) 
        {
            if (WarningExists) 
            {
                WarningSpawned.transform.position = new Vector2(WarningSpawned.transform.position.x, RefPlayer.transform.position.y); 
            }
            if (ParticlesExists)
            {
                AvalancheSpawned.transform.position = new Vector2(AvalancheSpawned.transform.position.x, RefPlayer.transform.position.y + DistanceFromPlayer);
            }
        }
    }

    private void SetRandomSpawnTime() 
    {
        AvalancheSpawnTime = Random.Range(AvalancheSpawnTime - 1f, AvalancheSpawnTime + 2f);
        AvalancheTimer = 0;
    }

    private void SpawnAvalanche() 
    {
        Debug.Log("Spawning avalanche");
        AvalancheExists = true;
        WarningSpawned = Instantiate(AvalancheWarningPrefab, new Vector2(Random.Range(MinX, MaxX), RefPlayer.transform.position.y + DistanceFromPlayer), Quaternion.identity);
        WarningExists = true;
        RefSprite = WarningSpawned.GetComponent<SpriteRenderer>();
        StartCoroutine(AvalancheWarning());
    }

    private IEnumerator AvalancheWarning() 
    {
        for (int i = 0; i < 3; i++) 
        {
            Color objectColor = RefSprite.material.color;
            while (objectColor.a >= 0) 
            {
                float fadeAmount = objectColor.a - (25f * Time.deltaTime);
                objectColor.a = fadeAmount;
                RefSprite.material.color = objectColor;
                //yield return new WaitForEndOfFrameUnit();
            }
            yield return new WaitForSeconds(.2f);
            while (objectColor.a <= .75f)
            {
                float fadeAmount = objectColor.a + (25f * Time.deltaTime);
                objectColor.a = fadeAmount;
                //yield return new WaitForEndOfFrameUnit();
                RefSprite.material.color = objectColor;
            }
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(2);
        AvalancheSpawned = Instantiate(AvalanchePrefab, WarningSpawned.transform.position, Quaternion.identity);
        ParticlesExists = true;
        WarningExists = false;
        Destroy(WarningSpawned);
        RefParticles = AvalancheSpawned.GetComponent<ParticleSystem>();
        RefParticles.Play();
        yield return new WaitForSeconds(8);
        ParticlesExists = false;
        AvalancheExists = false;
        Destroy(AvalancheSpawned);
    }
}