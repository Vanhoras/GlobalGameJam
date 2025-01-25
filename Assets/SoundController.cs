using System;
using System.Collections.Generic;
using UnityEngine;

public enum SfxIdentifier
{
    Jump,
    Land,
    Shoot,
    BubbleWallPop,
    BubblePlayerHit,
    Player1Win,
    Player2Win
}

[Serializable]
public struct SfxMapping
{
    public SfxIdentifier Key;
    public AudioClip AudioClip;
}

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private List<SfxMapping> mapping;

    [SerializeField]
    private AudioSource audioSource;

    private static SoundController instance;

    public static SoundController Instance => instance;

    public void PlaySound(SfxIdentifier key)
    {
        foreach (var entry in mapping)
        {
            if (entry.Key != key)
            {
                continue;
            }

            if(entry.AudioClip == null)
            {
                Debug.LogError("Audio clip with key " + key + " was null");
                return;
            }

            audioSource.PlayOneShot(entry.AudioClip);
        }

        Debug.LogError("No audio clip with key " + key + " can be found");
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
