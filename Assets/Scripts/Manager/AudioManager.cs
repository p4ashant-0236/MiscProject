using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioDataSO soundData;         // ScriptableObject holding audio clips for various types
    [SerializeField] private AudioSource musicSource;        // Dedicated audio source for background music
    [SerializeField] private List<AudioSource> sfxPool = new List<AudioSource>(); // Pool of sources for sound effects

    private const int FALLBACK_INDEX = 0;  // Used when all audio sources in the pool are busy

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Plays the specified audio clip based on type (background or SFX).
    /// </summary>
    /// <param name="type">The audio type to play.</param>
    public void PlaySound(AudioType type)
    {
        AudioClip clip = soundData.GetClip(type);
        if (clip == null) return;

        // Handle background music separately
        if (type == AudioType.BackgroundMusic)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
            return;
        }

        // Play sound effect using available source
        foreach (var src in sfxPool)
        {
            if (!src.isPlaying)
            {
                src.PlayOneShot(clip);
                return;
            }
        }

        // Fallback: If all sources are playing, reuse the first one
        if (sfxPool.Count > FALLBACK_INDEX)
        {
            sfxPool[FALLBACK_INDEX].PlayOneShot(clip);
        }
    }
}
