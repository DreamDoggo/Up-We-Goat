using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{
    [SerializeField] string DeathTriggerName = "Death Platform";

    Transform DeathTrigger;

    private void Awake()
    {
        if (SceneManager.GetSceneByName("sketch-leap").isLoaded) 
        {
            DeathTrigger = GameObject.Find(DeathTriggerName).transform;
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetSceneByName("sketch-leap").isLoaded) 
        { 
            if (transform.position.y < DeathTrigger.position.y) 
            {
                Destroy(gameObject);
            }
        }
    }

}
