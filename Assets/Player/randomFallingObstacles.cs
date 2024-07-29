using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomFallingObstacles : MonoBehaviour
{
    [SerializeField] GameObject[] Obstacle = new GameObject[2];
    LevelManager levelman;
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
    void Start(){
        levelman = FindFirstObjectByType<LevelManager>();
    }
    void Update(){
        timer = timer + Time.deltaTime;
        int level = LevelManager.Level-1;
        Vector3 point = new Vector3(Random.Range(MinX, MaxX), Player.transform.position.y + DistanceY, Player.transform.position.z);
        if (timer >= DropTimer){
            try{
                Instantiate(Obstacle[level], point, Player.transform.rotation);
                timer = 0;
            } catch {
            }
        }
    }
}
