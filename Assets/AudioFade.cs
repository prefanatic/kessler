using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    public float fadeTime;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        StartCoroutine(FadeAudio(audioSource, fadeTime, 0f));
        audioSource.Play();
    }

    public static IEnumerator FadeAudio(AudioSource source, float fadeTime, float delayTime)
    {
        var startVolume = source.volume;
        yield return new WaitForSeconds(delayTime);
        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        source.Stop();
        source.volume = startVolume;
    }
}
