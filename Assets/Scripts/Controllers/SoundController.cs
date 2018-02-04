using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip springSound;
    public AudioClip victorySound;

    public AudioSource audioSource;

    //would it gbe better to map a sound to an enum and have a single method that takes an sfx enum?
    public void PlayJumpSFX()
    {
        if(springSound != null)
            audioSource.PlayOneShot(springSound);
    }

    public void PlayVictorySFX()
    {
        if(victorySound != null)
            audioSource.PlayOneShot(victorySound);
    }
}
