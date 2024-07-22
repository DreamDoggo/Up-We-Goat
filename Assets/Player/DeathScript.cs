using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    float prevPos;
    float currentPos;
    void Start(){
        prevPos = player.transform.position.y;
    }

    void Update(){
        if (currentPos != prevPos){
            gameObject.transform.y = currentPos + prevPos;
            currentPos = prevPos;
        }
    }
}
