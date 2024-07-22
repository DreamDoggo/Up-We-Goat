using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode MoveLeftKey = KeyCode.A;
    [SerializeField] KeyCode MoveRightKey = KeyCode.D;
    [SerializeField] float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMovement() 
    {

        if (Input.GetKey(MoveLeftKey)) 
        {
            
        }
    }
    void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "obs"){
            Destroy(gameObject);
        }
    }
}
