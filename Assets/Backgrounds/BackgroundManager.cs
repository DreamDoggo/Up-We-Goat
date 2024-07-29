using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] Backgrounds = new GameObject[4];

    [Tooltip("How high the player needs to be before initiating the infinite spawing backgrounds")]
    [SerializeField] float infiniteBegin = 0;

    [Tooltip("How high the first backdrop is placed")]
    [SerializeField] float infinitePlacementBegin = 0;
    
    [SerializeField] int numberOfDupes = 0;
    float colliderHolder = 0;
    void Update()
    {
        if (player.transform.position.y >= infiniteBegin + colliderHolder * numberOfDupes){
            int rand = UnityEngine.Random.Range(0,Backgrounds.Length);
            BoxCollider2D backgroundCollider = Backgrounds[rand].GetComponent<BoxCollider2D>();
            GameObject newBG = Instantiate(Backgrounds[rand], new Vector3(0, infinitePlacementBegin + colliderHolder, 0), Quaternion.identity);
            newBG.GetComponent<SpriteRenderer>().sortingOrder = -1;
            colliderHolder = colliderHolder + backgroundCollider.size.y;
            numberOfDupes++;
        }

    }
}
