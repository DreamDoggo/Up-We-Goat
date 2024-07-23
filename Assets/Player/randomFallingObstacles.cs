using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomFallingObstacles : MonoBehaviour
{
    [SerializeField] GameObject Obstacle;
    [SerializeField] GameObject Player;
    [Range(-10f, 10f)]
    [SerializeField] float MinX;
    [Range(-10f, 10f)]
    [SerializeField] float MaxX;
    [Range(0f, 10f)]
    [SerializeField] float DistanceY;
    [Range(0f, 10f)]
    [SerializeField] float DropTimer = 10;
    [SerializeField] float timer;
    void Update(){
        timer = timer + Time.deltaTime;
        Vector3 point = new Vector3(Random.Range(MinX, MaxX),Player.transform.position.y+DistanceY,Player.transform.position.z);
        if (timer >= DropTimer){
            Instantiate(Obstacle, point, Player.transform.rotation);
            timer = 0;
        }
    }
}
