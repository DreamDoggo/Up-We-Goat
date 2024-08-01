using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Platform : MonoBehaviour
{
    [SerializeField] string DeathTriggerName = "Death Platform";

    Transform DeathTrigger = null;

    private void Awake()
    {
        if (SceneManager.GetSceneByName("sketch-leap").isLoaded) 
        {
            DeathTrigger = GameObject.Find(DeathTriggerName).transform;
        }
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetSceneByName("sketch-leap").isLoaded && DeathTrigger != null) 
        { 
            if (transform.position.y <= DeathTrigger.position.y+.25) 
            {
                Destroy(gameObject);
            }
        }
    }

}
