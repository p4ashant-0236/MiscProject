using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/Sound Data")]
public class AudioDataSO : ScriptableObject
{
    [System.Serializable]
    public class AudioEntry
    {
        public AudioType type;
        public AudioClip clip;
    }

    public List<AudioEntry> audios;

    private Dictionary<AudioType, AudioClip> audioMap;

    public void Initialize()
    {
        audioMap = new Dictionary<AudioType, AudioClip>();
        foreach (var entry in audios)
        {
            if (!audioMap.ContainsKey(entry.type))
                audioMap.Add(entry.type, entry.clip);
        }
    }

    public AudioClip GetClip(AudioType type)
    {
        if (audioMap == null)
            Initialize();

        if (audioMap.TryGetValue(type, out var clip))
            return clip;

        Debug.LogWarning($"AudioType {type} not found!");
        return null;
    }
}

public enum AudioType
{
    Flip,
    Match,
    Mismatch,
    GameComplete,
    BackgroundMusic
}