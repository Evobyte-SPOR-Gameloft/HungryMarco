using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource soundEffect;
    void Start()
    {
        soundEffect = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    void PlayAudio(string filename)
    {
        soundEffect.clip = Resources.Load<AudioClip>("Audioclips/" + filename);
        soundEffect.Play();
    }

}//class
