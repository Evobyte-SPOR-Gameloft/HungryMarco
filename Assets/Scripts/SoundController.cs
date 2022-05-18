using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource soundEffect;
    void Start()
    {
        soundEffect = GetComponent<AudioSource>();

        NewPlayer.instance.PlayerDiedInfo += PlayerDiedListener;
        NewPlayer.instance.PlayerAteInfo += PlayerAteListener;
    }

    void PlayAudio(string filename)
    {
        soundEffect.clip = Resources.Load<AudioClip>("Audioclips/" + filename);
        soundEffect.Play();
    }

    void PlayerDiedListener()
    {
        Debug.Log("Death event called");
        PlayAudio("deathSound");
    }

    void PlayerAteListener()
    {
        Debug.Log("Ate enemy event called");
        PlayAudio("crunchSound");
    }

}//class
