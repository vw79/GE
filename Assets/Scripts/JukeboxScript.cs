using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeboxScript : MonoBehaviour
{

    public AudioClip firstAudioClip;
    public AudioClip secondAudioClip;
    public float fadeDuration = 2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (firstAudioClip != null)
        {
            audioSource.clip = firstAudioClip;
            audioSource.Play();
        }
    }


    IEnumerator FadeAndPlay()
    {
        // Fade out the first audio clip
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, (Time.time - startTime) / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;

        // Change the audio clip and play the second audio clip
        audioSource.clip = secondAudioClip;
        audioSource.Play();

        // Fade in the second audio clip
        startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, (Time.time - startTime) / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
