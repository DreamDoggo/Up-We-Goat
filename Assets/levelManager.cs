using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The level which the player is currently in")]
    [SerializeField] public static int Level = 0;

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Player"){
            Level++;
            Destroy(gameObject);
        }
    }

}
