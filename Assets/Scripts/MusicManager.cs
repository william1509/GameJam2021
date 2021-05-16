using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource utopianSource;
    public AudioSource distopianSource;

    bool utopianPlaying = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAudioSources(AudioSource utopianSource, AudioSource distopianSource)
    {
        this.utopianSource = utopianSource;
        this.distopianSource = distopianSource;

        // Make music loop
        this.utopianSource.loop = true;
        this.distopianSource.loop = true;

        // Start playing music
        this.utopianSource.Play();
        this.distopianSource.Play();
        this.distopianSource.volume = 0.0f; // Distopian music starts at 0 volume
    }

    public void ToggleMusic()
    {
        if (utopianPlaying)
        {
            StartCoroutine(FadeAudioSource.StartFade(utopianSource, 2, 0.0f));
            StartCoroutine(FadeAudioSource.StartFade(distopianSource, 2, 1.0f));
            utopianPlaying = false;
        }
        else
        {
            StartCoroutine(FadeAudioSource.StartFade(utopianSource, 2, 1.0f));
            StartCoroutine(FadeAudioSource.StartFade(distopianSource, 2, 0.0f));
            utopianPlaying = true;
        }
    }
}