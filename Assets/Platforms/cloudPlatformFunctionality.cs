using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class cloudPlatformFunctionality : MonoBehaviour
{
    [SerializeField] float fadeTimer = .01f;
    void Start(){
    }
    void OnTriggerEnter2D(Collider2D coll){
        if (coll.name == "Player"){
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade(){
    Color color = GetComponent<Renderer>().material.color;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
    {
        color.a = alpha;
        GetComponent<Renderer>().material.color = color;
        yield return null;
    }
    Destroy(gameObject);    
    }
}
