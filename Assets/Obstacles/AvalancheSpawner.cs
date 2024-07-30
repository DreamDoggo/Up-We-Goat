using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheSpawner : MonoBehaviour
{
    [SerializeField] GameObject AvalanchePrefab;
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
    ParticleSystem RefParticles;
    SpriteRenderer RefSprite;

    private void Awake()
    {
        SetRandomSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Level == 2) 
        {
            if (AvalancheTimer >= AvalancheSpawnTime) 
            {
                SetRandomSpawnTime();
                SpawnAvalanche();
            }
            AvalancheTimer += Time.deltaTime;
        }
    }

    private void SetRandomSpawnTime() 
    {
        AvalancheSpawnTime = Random.Range(AvalancheTimer - 1f, AvalancheTimer + 2f);
        AvalancheTimer = 0;
    }

    private void SpawnAvalanche() 
    {
        AvalancheSpawned = Instantiate(AvalanchePrefab, new Vector2(Random.Range(MinX, MaxX), RefPlayer.transform.position.y + DistanceFromPlayer), Quaternion.identity);
        RefParticles = AvalancheSpawned.GetComponent<ParticleSystem>();
        RefParticles.Stop();
        RefSprite = AvalancheSpawned.GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(AvalancheWarning());
    }

    private IEnumerator AvalancheWarning() 
    {
        for (int i = 0; i < 2; i++) 
        {
            Color objectColor = RefSprite.material.color;
            while (objectColor.a >= 0) 
            {
                float fadeAmount = objectColor.a - (.1f * Time.deltaTime);
                objectColor.a = fadeAmount;
                RefSprite.material.color = objectColor;
            }
            yield return new WaitForSeconds(.2f);
            while (objectColor.a <= .75f)
            {
                float fadeAmount = objectColor.a + (.1f * Time.deltaTime);
                objectColor.a = fadeAmount;
                RefSprite.material.color = objectColor;
            }
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(1);
        RefSprite.material.color = Color.clear;
        RefParticles.Play();
        yield return new WaitForSeconds(10);
        Destroy(AvalancheSpawned);
    }
}
