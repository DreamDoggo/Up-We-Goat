using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] float deathTimer = 1;
    [SerializeField] string DeathTrigger = "Death Platform";
    public float MoveSpeed = 1f;
    Rigidbody2D RefRigidBody;
    SpriteRenderer RefSprite;
    CircleCollider2D RefCollider;

    public float LeftRotationAmount = 330f;
    public float RightRotationAmount = 210f;
    public bool ComingFromLeft;
    Quaternion LeftRotator;
    Quaternion RightRotator;

    private void Awake()
    {
        RefRigidBody = GetComponent<Rigidbody2D>();
        RefSprite = GetComponent<SpriteRenderer>();
        RefCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LeftRotator = Quaternion.AngleAxis(LeftRotationAmount, Vector3.forward);
        RightRotator = Quaternion.AngleAxis(RightRotationAmount, Vector3.forward);
        RefSprite.flipX = true;
        if (Mathf.Sign(transform.position.x) <= -1) 
        {
            ComingFromLeft = true;
            RefSprite.flipY = true;
            RefCollider.offset = new Vector2(1f, -.3f);
        }
        else 
        {
            ComingFromLeft = false;
            RefSprite.flipY = false;
            RefCollider.offset = new Vector2(1f, .3f);
        }

        if (ComingFromLeft) 
        {
            transform.rotation = LeftRotator;
        }
        else 
        {
            transform.rotation = RightRotator;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement() 
    {
        RefRigidBody.velocity = transform.right * MoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.name == DeathTrigger)
            {
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }
}
