using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 playerPrevPos;
    // Saves player position on start then changes if different -5 from currentPos
    Vector3 currentPos;
    // Saves current position of the platform
    public float distance = 5;
    // Distance between the player and death platform

    void Start(){
        playerPrevPos = player.transform.position;
        currentPos = gameObject.transform.position;
    }

    void Update(){
        if ((playerPrevPos.y - distance) > currentPos.y){
            transform.position = new Vector3(currentPos.x, playerPrevPos.y - distance, currentPos.z);
            currentPos = gameObject.transform.position;
        }
    }
}
