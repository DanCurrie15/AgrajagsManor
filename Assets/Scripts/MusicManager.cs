using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioClip song;
    public AudioSource MusicSource;

    public void PlayMusic()
    {
        MusicSource.clip = song;
        MusicSource.Play();      
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    private void Start()
    {
        PlayMusic();
    }
}
