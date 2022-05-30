using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<SoundFXDefinition> SoundFX;
    public AudioSource SoundFXSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        AudioClip effect = SoundFX.Find(sfx => sfx.Effect == soundEffect).Clip;
        SoundFXSource.PlayOneShot(effect);
    }
}