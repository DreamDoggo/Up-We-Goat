using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource Footstep;
    [SerializeField] AudioClip Step;
    public void PlayStep()
    {
        AudioSource.PlayClipAtPoint(Step, transform.position);
    }  
}
