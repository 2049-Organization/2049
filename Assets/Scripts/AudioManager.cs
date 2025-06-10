using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource.clip != clip)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
            Invoke(nameof(StopSFX), clip.length);
        }
    }
    public void StopSFX()
    {
        sfxSource.clip = null;
        sfxSource.Stop();
    }
}
