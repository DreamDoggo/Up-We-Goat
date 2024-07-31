using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cloudPlatformFunctionality : MonoBehaviour
{
    Color color;
    [SerializeField] float fadeTimer = .25f;
    void Start(){
    }
    void OnTriggerEnter2D(Collider2D coll){
        if (coll.name == "Player"){
            while(color.a > 0){
                StartCoroutine(Fade());
            }
        }
    }

    IEnumerator Fade(){
    Color c = GetComponent<Material>().color;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            renderer.material.color = c;
            yield return null;
        }
}
}
