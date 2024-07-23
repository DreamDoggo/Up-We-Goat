using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Keybinds")]
    [SerializeField] KeyCode MoveLeftKey = KeyCode.A;
    [SerializeField] KeyCode AltMoveLeftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode MoveRightKey = KeyCode.D;
    [SerializeField] KeyCode AltMoveRightKey = KeyCode.RightArrow;
    [SerializeField] KeyCode JumpKey = KeyCode.Space;
    [SerializeField] KeyCode AltJumpKey = KeyCode.W;

    [Header("Horizontal Movement")]
    [Range(2f, 30f)]
    [SerializeField] float MoveSpeed = 8f;

    [Tooltip("The player cannot exceed this speed horizontally")]
    [Range(2f, 30f)]
    [SerializeField] float MaximumVelocity = 5f;

    [Tooltip("How quickly to slow the player down after movement keys are not held down" +
        "Lower values produce larger results")]
    [Range(.1f, 1f)]
    [SerializeField] float MovementDamping = .95f;

    [Header("Jump")]
    [Range(2f, 60f)]
    [SerializeField] float JumpStrength = 10f;

    [Tooltip("How much to lower the player's jump by if they let go of the jump key early" +
        "Lower values produce larger results")]
    [Range(.1f, 1f)]
    [SerializeField] float JumpDamping = .5f;

    [Header("References")]
    [SerializeField] SpriteRenderer RefSprite;
    [SerializeField] Rigidbody2D RefRigidBody;
    [SerializeField] BoxCollider2D RefCollider;

    [Header("Misc")]
    [Tooltip("Tag that hazardous objects will be marked with")]
    [SerializeField] string HazardTag = "obs";

    [Tooltip("Where on the player should they check to see if they are grounded")]
    [SerializeField] Transform GroundCheck;

    [Tooltip("What layer is considered to be ground")]
    [SerializeField] LayerMask GroundLayer;
    
    [SerializeField] AudioSource JumpSource;
    [SerializeField] AudioClip JumpSFX;

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    public void Move() 
    {
        // Check to see if we are holding left or right
        Vector2 horizontalInputs = Vector2.zero;

        if (Input.GetKey(MoveLeftKey) || Input.GetKey(AltMoveLeftKey)) 
        {
            horizontalInputs += Vector2.left;
            RefSprite.flipX = true;
        }
        if (Input.GetKey(MoveRightKey) || Input.GetKey(AltMoveRightKey)) 
        {
            horizontalInputs += Vector2.right;
            RefSprite.flipX = false;
        }
        horizontalInputs.Normalize();

        // Move the player in the direction
        RefRigidBody.AddForce(horizontalInputs * MoveSpeed);

        // Cap the player's velocity if it exceeds our maximum
        if (RefRigidBody.velocity.magnitude > MaximumVelocity) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.normalized.x * MaximumVelocity, RefRigidBody.velocity.y);
        }

        // Slow they player down if we stop receiving inputs
        if (horizontalInputs.sqrMagnitude <= 0.1f) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x * MovementDamping, RefRigidBody.velocity.y);
        }
    }

    // If on the ground and pressing jump, jump as normal
    // If they are jumping and let go of jump early, damp their upward velocity 

    public void Jump() 
    {
        if ((Input.GetKey(JumpKey) || Input.GetKey(AltJumpKey)) && IsGrounded()) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x, JumpStrength);
            if (JumpSource.isPlaying == false)
            {
                JumpSource.PlayOneShot(JumpSFX);
            }
        }

        if ((Input.GetKeyUp(JumpKey) || Input.GetKeyUp(AltJumpKey)) && RefRigidBody.velocity.y > 0f) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x, RefRigidBody.velocity.y * JumpDamping);
        }

    }

    // If the player is close enough to the ground, consider them to be grounded
    private bool IsGrounded() 
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.tag == HazardTag)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
