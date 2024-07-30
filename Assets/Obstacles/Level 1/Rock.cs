using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [Tooltip("How much to rotate the rocks by as they fall")]
    [SerializeField] Vector3 RotationAmount;
    [Tooltip("What object should the rock be destoyed when it touches")]
    [SerializeField] string DeathTrigger = "Death Platform";
    [SerializeField] float deathTimer = 1;

    // Update is called once per frame
    void Update()
    {
        UpdateSpin();
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
    private void UpdateSpin() 
    {
        transform.Rotate(RotationAmount * Time.deltaTime);
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }
}
