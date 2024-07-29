using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theGraciousBird : MonoBehaviour
{
    public float speed;
    void Update()
    {
        gameObject.transform.position += Vector3.right * speed/100;
    }


}
