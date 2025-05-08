using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioDataSO soundData;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioSource> sfxPool = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioType type)
    {
        AudioClip clip = soundData.GetClip(type);
        if (clip == null) return;

        if (type == AudioType.BackgroundMusic)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();

            return;
        }

        foreach (var src in sfxPool)
        {
            if (!src.isPlaying)
            {
                src.PlayOneShot(clip);
                return;
            }
        }

        // All busy? Use fallback
        sfxPool[0].PlayOneShot(clip);

    }
}