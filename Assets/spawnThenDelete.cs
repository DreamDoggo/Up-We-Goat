using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnThenDelete : MonoBehaviour
{
    [SerializeField] float timeBeforeRemoval = 0.5f;
    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timeBeforeRemoval <= timer){
            Destroy(gameObject);
        }
    }
}
