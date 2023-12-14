using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip gemSFX;
    public AudioClip powerUpSFX;

    public void PlaySFX(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
