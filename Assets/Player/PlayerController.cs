using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Keybinds")]
    [SerializeField] KeyCode MoveLeftKey = KeyCode.A;
    [SerializeField] KeyCode MoveRightKey = KeyCode.D;
    [SerializeField] KeyCode JumpKey = KeyCode.Space;

    [SerializeField] float MoveSpeed;

    [SerializeField] SpriteRenderer RefSprite;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    public void UpdateMovement() 
    {

        if (Input.GetKey(MoveLeftKey)) 
        {
            RefSprite.flipX = true;
        }
        if (Input.GetKey(MoveRightKey)) 
        {
            RefSprite.flipX = false;
        }
    }
}
