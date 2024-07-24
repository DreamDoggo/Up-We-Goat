using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Keybinds")]
    [SerializeField] KeyCode MoveLeftKey = KeyCode.A;
    [SerializeField] KeyCode MoveRightKey = KeyCode.D;
    [SerializeField] KeyCode JumpKey = KeyCode.Space;
    [SerializeField] KeyCode AltMoveLeftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode AltMoveRightKey = KeyCode.RightArrow;
    [SerializeField] KeyCode AltJumpKey = KeyCode.W;

    [Header("Horizontal Movement")]
    [Range(2f, 60f)]
    [SerializeField] float MoveSpeed = 8f;

    [Tooltip("The player cannot exceed this speed horizontally")]
    [Range(2f, 60f)]
    [SerializeField] float MaximumVelocity = 5f;

    [Tooltip("How quickly to slow the player down after movement keys are not held down" +
        "Lower values produce larger results")]
    [Range(-1f, 3f)]
    [SerializeField] float MovementDamping = .95f;
    
    [Tooltip("How quickly to slow the player down after movement keys are not held down" +
        "whilst on icey platforms")]
    [Range(-1, 3f)]
    [SerializeField] float IceyMovementDamping = 1f;

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
    [SerializeField] Text CollectionText;

    [Header("Misc")]
    [Tooltip("How many collectable thingies the player has collected")]
    [SerializeField] int grabby = 0;
    [Tooltip("How much time it takes for the player to die")]
    [Range(0f, 10f)]
    [SerializeField] float timeBeforeDeath = 5;
    [Tooltip("Tag that hazardous objects will be marked with")]
    [SerializeField] string HazardTag = "obs";
    [Tooltip("Tag that icey objects will be marked with")]
    [SerializeField] string IceyTag = "slippery";
    [Tooltip("Tag that grabbable objects will be marked with")]
    [SerializeField] string GrabTag = "grab";

    [Tooltip("Where on the player should they check to see if they are grounded")]
    [SerializeField] Transform GroundCheck;

    [Tooltip("What layer is considered to be ground")]
    [SerializeField] LayerMask GroundLayer;
    
    [SerializeField] AudioSource JumpSource;
    [SerializeField] AudioClip JumpSFX;
    float floatTemp;

    //
    public Animator animator;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

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
        //RefRigidBody.AddForce(horizontalInputs * MoveSpeed);
        RefRigidBody.velocity = new Vector2(horizontalInputs.x * MoveSpeed, RefRigidBody.velocity.y);

        // Cap the player's velocity if it exceeds our maximum
        if (RefRigidBody.velocity.magnitude > MaximumVelocity) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.normalized.x * MaximumVelocity, RefRigidBody.velocity.y);
        }

        // Slow they player down if we stop receiving inputs
        if (horizontalInputs.sqrMagnitude <= 0.1f) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x * MovementDamping * Time.deltaTime, RefRigidBody.velocity.y);
        }
    }

    // If on the ground and pressing jump, jump as normal
    // If they are jumping and let go of jump early, damp their upward velocity 

    public void Jump() 
    {
        if ((Input.GetKeyDown(JumpKey) || Input.GetKey(AltJumpKey)) && IsGrounded()) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x, JumpStrength);
            if (JumpSource.isPlaying == false)
            {
                JumpSource.PlayOneShot(JumpSFX);
            }
        }

        if ((Input.GetKeyUp(JumpKey) || Input.GetKeyUp(AltJumpKey)) && RefRigidBody.velocity.y > 0f) 
        {
            RefRigidBody.velocity *= new Vector2(1, JumpDamping);
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
            StartCoroutine(death());
        }
        else if (coll.tag == IceyTag){
            floatTemp = MovementDamping;
            MovementDamping = IceyMovementDamping;
        }
        else if (coll.tag == GrabTag){
            grabby++;
            CollectionText.text = grabby.ToString(); 
            Destroy (coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll){
        if (coll.tag == IceyTag){
            MovementDamping = floatTemp;
        }
    }

    IEnumerator death(){
        yield return new WaitForSeconds(timeBeforeDeath);
        SceneManager.LoadScene("GameOver");
    }
}
