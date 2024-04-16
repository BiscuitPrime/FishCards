using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will manage the cards' audio : if any card wants to play an audio, it calls this one.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class CardAudioManager : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static CardAudioManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }
    #endregion
    private AudioSource _audioSource;

    public void PlayAudioClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
