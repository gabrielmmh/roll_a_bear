using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    public AudioClip walkSound;
    public AudioClip runSound;

    public Transform player;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Debug.Log("runForward: " + player.GetComponent<PlayerController>().runForward);

        if (player.GetComponent<PlayerController>().runForward)
        {
            audioSource.clip = runSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (player.GetComponent<PlayerController>().walkForward || player.GetComponent<PlayerController>().walkBackward)
        {
            audioSource.clip = walkSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        else if (!player.GetComponent<PlayerController>().walkForward && !player.GetComponent<PlayerController>().walkBackward && !player.GetComponent<PlayerController>().runForward)
        {
            audioSource.Stop();
        }
    }
}
