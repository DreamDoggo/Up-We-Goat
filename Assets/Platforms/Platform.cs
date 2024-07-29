using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] string DeathTriggerName = "Death Platform";

    Transform DeathTrigger;

    private void Awake()
    {
        DeathTrigger = GameObject.Find(DeathTriggerName).transform;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < DeathTrigger.position.y) 
        {
            Destroy(gameObject);
        }
    }

}
