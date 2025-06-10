using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource.clip != clip)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
            Invoke(nameof(StopSFX), clip.length+0.1f);
        }
    }
    public void StopSFX()
    {
        sfxSource.clip = null;
        sfxSource.Stop();
    }
}
