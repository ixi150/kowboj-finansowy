using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToPlay : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    AudioSource audioSource;

    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    public void Play()
    {
        var clip = clips[Random.Range(0, clips.Length)];
        var volume = Random.Range(.9f, 1);
        audioSource.PlayOneShot(clip, volume);
    }
}
