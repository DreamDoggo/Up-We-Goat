using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    [Tooltip("The level which the player is currently in")]
    [SerializeField] public static int level = 0;
    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Player"){
            level++;
            Destroy(gameObject);
        }
    }
}
