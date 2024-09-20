using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleSounds : MonoBehaviour
{
    public AudioClip jungleSound1;    // Som de floresta 1
    public AudioClip jungleSound2;    // Som de floresta 2
    public AudioClip jungleSound3;    // Som de floresta 3
    public AudioClip jungleSound4;    // Som de floresta 4  
    public AudioClip jungleSound5;    // Som de floresta 5

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomJungleSound();
    }

    public void PlayRandomJungleSound()
    {
        int randomIndex = Random.Range(0, 5);

        switch (randomIndex)
        {
            case 0:
                audioSource.PlayOneShot(jungleSound1);
                break;
            case 1:
                audioSource.PlayOneShot(jungleSound2);
                break;
            case 2:
                audioSource.PlayOneShot(jungleSound3);
                break;
            case 3:
                audioSource.PlayOneShot(jungleSound4);
                break;
            case 4:
                audioSource.PlayOneShot(jungleSound5);
                break;
        }
    }
}
