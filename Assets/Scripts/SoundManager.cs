using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public List<AudioClip> clipList;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(int audioClipIndex)
    {
        audioSource.clip = clipList[audioClipIndex];
        audioSource.Play();
    }
}
