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

    [SerializeField] Vector2 NextBackgroundPosition;

    /*void Update()
    {
        if (player.transform.position.y >= infiniteBegin + colliderHolder * numberOfDupes){
            int rand = UnityEngine.Random.Range(0,Backgrounds.Length);
            BoxCollider2D backgroundCollider = Backgrounds[rand].GetComponent<BoxCollider2D>();
            GameObject newBG = Instantiate(Backgrounds[rand], new Vector3(0, infinitePlacementBegin + colliderHolder, 0), Quaternion.identity);
            newBG.GetComponent<SpriteRenderer>().sortingOrder = -2;
            colliderHolder = colliderHolder + backgroundCollider.size.y;
            numberOfDupes++;
        }

    }*/

    public void PlaceInfiniteBackgrounds() 
    {
        for (int i = 0; i <= 8; i++) 
        {
            Instantiate(PickRandomBackground(), NextBackgroundPosition, Quaternion.identity);
            NextBackgroundPosition.y += 10f;
        }
    }

    private GameObject PickRandomBackground() 
    {
        int background = UnityEngine.Random.Range(0, Backgrounds.Length);
        return Backgrounds[background];
    }
}
