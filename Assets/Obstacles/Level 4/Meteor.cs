using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] float deathTimer = 1;
    [SerializeField] string DeathTrigger = "Death Platform";
    public float MoveSpeed = 1f;
    Rigidbody2D RefRigidBody;

    public float LeftRotationAmount = 330f;
    public float RightRotationAmount = 210f;
    public bool ComingFromLeft;
    Quaternion LeftRotator;
    Quaternion RightRotator;

    private void Awake()
    {
        RefRigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LeftRotator = Quaternion.AngleAxis(LeftRotationAmount, Vector3.forward);
        RightRotator = Quaternion.AngleAxis(RightRotationAmount, Vector3.forward);
        if (Mathf.Sign(transform.position.x) <= -1) 
        {
            ComingFromLeft = true;
        }
        else 
        {
            ComingFromLeft = false;
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
