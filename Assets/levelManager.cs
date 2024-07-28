using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The level which the player is currently in")]
    [SerializeField] public static int Level = 0;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Player"){
            Level++;
            gameManager.PlaceNewLevel();
            Destroy(gameObject);
        }
    }
}
