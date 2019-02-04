using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioClip music;

    [SerializeField] float minimumTimeBetweenSongs; // seconds between end of one song before the next starts

    float timeUntilCurrentSongEnds = 0.0f;
    float timeSinceCurrentSongEnded = 0.0f;
    float timeSinceLastRandomSongAttempt = 0.0f;

    float musicVolume = 0.188f;

    Dictionary<AudioSource, float> timeToDestroyAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        timeToDestroyAudioSource = new Dictionary<AudioSource, float>();
    }

    private void Update()
    {
        if (timeUntilCurrentSongEnds <= 0.0f)
        {
            timeSinceCurrentSongEnded += Time.deltaTime;
            if (timeSinceCurrentSongEnded >= minimumTimeBetweenSongs)
            {
                // Only try to start a new song every second
                timeSinceLastRandomSongAttempt += Time.deltaTime;
                if (timeSinceLastRandomSongAttempt >= 1.0f)
                {
                    timeSinceLastRandomSongAttempt = 0.0f;
                    if (Random.Range(0, 100) >= 19)
                    {
                        StartMusic();
                    }
                }
            }
        }
        else
        {
            timeUntilCurrentSongEnds -= Time.deltaTime;
        }

        // Check all the audio sources and see if they should be destroyed
        List<AudioSource> toRemove = new List<AudioSource>();
        foreach (AudioSource audioSource in timeToDestroyAudioSource.Keys)
        {
            if (Time.time > timeToDestroyAudioSource[audioSource])
            {
                toRemove.Add(audioSource);
            }
        }
        foreach (AudioSource audioSource in toRemove)
        {
            timeToDestroyAudioSource.Remove(audioSource);
            Destroy(audioSource);
        }
    }

    /// <summary>
    /// Adds an AudioSource with the included clip.
    /// </summary>
    /// <param name="clip">The clip to be played.</param>
    /// <param name="volume">How loud the clip should play (0.0-1.0).</param>
    /// <param name="dopplerLevel">The doppler level (0.0-1.0).</param>
    void StartSound(AudioClip clip, float volume = 1.0f, float dopplerLevel = 0.0f)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.volume = volume;
        source.dopplerLevel = dopplerLevel;
        source.Play();
        timeToDestroyAudioSource.Add(source, Time.time + music.length);
    }
    
    /// <summary>
    /// Starts a random song (we only have one now though :))
    /// </summary>
    void StartMusic()
    {
        StartSound(music, 0.188f, 0.0f);
        timeUntilCurrentSongEnds = music.length;
    }

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        foreach (AudioSource source in timeToDestroyAudioSource.Keys)
        {
            Debug.Log(source.name);
            if (source.clip == music)
            {
                source.volume = musicVolume;
            }
        }
    }
}
