using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] KeyCode SecondAltJumpKey = KeyCode.UpArrow;

    [Header("Horizontal Movement")]
    [Range(2f, 60f)]
    [SerializeField] float MoveSpeed = 8f;

    [Tooltip("The player cannot exceed this speed horizontally")]
    [Range(2f, 60f)]
    [SerializeField] float MaximumVelocity = 5f;

    [Tooltip("How quickly to slow the player down after movement keys are not held down on ice" +
        "Lower values produce larger results")]
    // Relic of the old force based movement system
    [Range(-1f, 3f)]
    [SerializeField] float IceMovementDamping = .98f;
    
    [Tooltip("How quickly to slow the player down after movement keys are not held down" +
        "whilst on icey platforms")]
    [Range(1.1f, 5f)]
    [SerializeField] float IceMoveSpeed = 1.2f;

    [Header("Jump")]
    [Range(2f, 60f)]
    [SerializeField] float JumpStrength = 10f;

    [Tooltip("How much to lower the player's jump by if they let go of the jump key early" +
        "Lower values produce larger results")]
    [Range(.1f, 1f)]
    [SerializeField] float JumpDamping = .5f;

    [Tooltip("Time in seconds the player can still jump while not on the ground")]
    [Range(0f, .3f)]
    [SerializeField] float CoyoteTime = 0.1f;

    [Tooltip("Time in seconds the player can press jump before being on the ground and still jump")]
    [Range(0f, .3f)]
    [SerializeField] float JumpBufferTime = 0.1f;

    [Header("References")]
    [SerializeField] SpriteRenderer RefSprite;
    [SerializeField] Rigidbody2D RefRigidBody;
    [SerializeField] BoxCollider2D RefCollider;
    [SerializeField] Animator RefAnimator;
    [SerializeField] Text CollectionText;
    [SerializeField] AudioSource GoatSource;
    [SerializeField] AudioSource JumpSource; 
    [SerializeField] GameObject JumpParticle; 

    [Header("Misc")]
    [Tooltip("How many collectable thingies the player has collected")]
    [SerializeField] public static int collectables = 0;
    [Tooltip("How much time it takes for the player to die")]
    [Range(0f, 10f)]
    [SerializeField] float TimeBeforeDeath = 5;
    [Tooltip("Tag that hazardous objects will be marked with")]
    [SerializeField] string HazardTag = "obs";
    [Tooltip("Tag that icey objects will be marked with")]
    [SerializeField] string IcyTag = "slippery";
    [Tooltip("Tag that grabbable objects will be marked with")]
    [SerializeField] string GrabTag = "grab";

    [Tooltip("Where on the player should they check to see if they are grounded")]
    [SerializeField] Transform GroundCheck;

    [Tooltip("What layer is considered to be ground")]
    [SerializeField] LayerMask GroundLayer;

    [Header("Sound Effects")]
    [SerializeField] AudioClip JumpSFX;
    [SerializeField] AudioClip CollectableSFX;
    private float CoyoteTimeCounter;
    private float JumpBufferCounter;
    private bool OnIce;
    private int landPart = 0;

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseButton.GameIsPaused) { return; }
        Move();
        Jump();
        AnimationChecks();
    }

    public void Move() 
    {
        // Check to see if we are holding left or right
        Vector2 horizontalInputs = Vector2.zero;

        if (Input.GetKey(MoveLeftKey) || Input.GetKey(AltMoveLeftKey)) 
        {
            RefAnimator.SetFloat("Speed",1);
            horizontalInputs += Vector2.left;
            RefSprite.flipX = true;
        }
        else if (Input.GetKey(MoveRightKey) || Input.GetKey(AltMoveRightKey)) 
        {
            RefAnimator.SetFloat("Speed",1);
            horizontalInputs += Vector2.right;
            RefSprite.flipX = false;
        } else {
            RefAnimator.SetFloat("Speed",0);
        }
        horizontalInputs.Normalize();

        // Move the player in the direction
        // Old force based code: RefRigidBody.AddForce(horizontalInputs * MoveSpeed);
        if (OnIce) 
        {
            RefRigidBody.AddForce(horizontalInputs * MoveSpeed * IceMoveSpeed);
        }
        else 
        {
            RefRigidBody.velocity = new Vector2(horizontalInputs.x * MoveSpeed, RefRigidBody.velocity.y);
        }


        // Cap the player's velocity if it exceeds our maximum
        if (RefRigidBody.velocity.magnitude > MaximumVelocity) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.normalized.x * MaximumVelocity, RefRigidBody.velocity.y);
            if (horizontalInputs.sqrMagnitude <= 0.1f) 
            {
                RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x * IceMovementDamping * Time.deltaTime, RefRigidBody.velocity.y);
            }
        }

        // Slow they player down if we stop receiving inputs
        // Old forced based code:
        /*if (horizontalInputs.sqrMagnitude <= 0.1f) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x * MovementDamping * Time.deltaTime, RefRigidBody.velocity.y);
        }*/
    }

    // If on the ground and pressing jump, jump as normal
    // If they are jumping and let go of jump early, damp their upward velocity 

    public void Jump() 
    {
        // Handle Coyote Time
        if (IsGrounded()) 
        {
            RefAnimator.SetBool("isAirborne", false);
            CoyoteTimeCounter = CoyoteTime;
            if (landPart == 1) {
                jumpParticle();
                landPart = 0;
            }
        }
        else 
        {
            RefAnimator.SetBool("isAirborne", true);
            CoyoteTimeCounter -= Time.deltaTime;
            landPart = 1;
        }
        
        // Handle Jump Buffering
        if (Input.GetKeyDown(JumpKey) || Input.GetKeyDown(AltJumpKey) || Input.GetKeyDown(SecondAltJumpKey)) 
        {
            jumpParticle();
            JumpBufferCounter = JumpBufferTime;
        }
        else 
        {
            JumpBufferCounter -= Time.deltaTime;
        }

        if (JumpBufferCounter > 0f && CoyoteTimeCounter > 0f) 
        {
            RefRigidBody.velocity = new Vector2(RefRigidBody.velocity.x, JumpStrength);
            JumpBufferCounter = 0f;
            if (JumpSource.isPlaying == false)
            {
                JumpSource.PlayOneShot(JumpSFX);
            }
        }

        if ((Input.GetKeyUp(JumpKey) || Input.GetKeyUp(AltJumpKey) || Input.GetKeyUp(SecondAltJumpKey)) && RefRigidBody.velocity.y > 0f) 
        {
            RefRigidBody.velocity *= new Vector2(1, JumpDamping);
            CoyoteTimeCounter = 0f;
        }
    }

    // If the player is close enough to the ground, consider them to be grounded
    public bool IsGrounded() 
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.tag == HazardTag)
        {
            if (collectables >= PlayerPrefs.GetInt("highscore")){
                PlayerPrefs.SetInt("highscore", collectables);
            }
            StartCoroutine(Death());
            
        }
        else if (coll.tag == IcyTag)
        {
            OnIce = true;
        }
        else if (coll.tag == GrabTag)
        {
            collectables++;
            CollectionText.text = collectables.ToString();
            AudioSource.PlayClipAtPoint(CollectableSFX, transform.position);
            GoatSource.PlayOneShot(CollectableSFX);
            Destroy (coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll){
        if (coll.tag == IcyTag)
        {
            OnIce = false;
        }
    }

    private void OnParticleCollision(GameObject particle)
    {
        if (particle.gameObject.tag == HazardTag) 
        {
            if (collectables >= PlayerPrefs.GetInt("highscore")){
                PlayerPrefs.SetInt("highscore", collectables);
            }
            StartCoroutine(Death());
        }
    }

    private void AnimationChecks() 
    {
        RefAnimator.SetFloat("yVelocity", RefRigidBody.velocity.y);
        if (RefRigidBody.velocity.y < 0 && IsGrounded()) 
        {
            RefAnimator.SetBool("isLanding", true);
        }
        else if (RefRigidBody.velocity.y > 0 && IsGrounded())
        {
            RefAnimator.SetBool("isLanding", false);
        }
    }

    void jumpParticle(){
        Instantiate(JumpParticle, new Vector3(GroundCheck.transform.position.x, GroundCheck.transform.position.y), Quaternion.identity);
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(TimeBeforeDeath);
        collectables = 0;
        SceneManager.LoadScene("GameOver");
        yield return null;
    }
}
