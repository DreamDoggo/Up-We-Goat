using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource Footstep;
    [SerializeField] AudioClip Step;
    public void PlayStep()
    {
        AudioSource.PlayClipAtPoint(Step, transform.position);
    }  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
