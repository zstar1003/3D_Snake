using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource SfxAudio;

    public AudioClip eat;
    
    public void PlaySfx(AudioClip clip)
    {
        SfxAudio.PlayOneShot(clip);
    }
}
