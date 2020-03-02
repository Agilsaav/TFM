using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WavesBehavior
{
    public class WaveSoundManager : MonoBehaviour
    {
        public void AssingClip(AudioSource audioSource, AudioClip clip)
        {
            if (clip != null) audioSource.clip = clip;
        }

        public void ChangeVolume(AudioSource audioSource, float volumeStep)
        {
            audioSource.volume -= volumeStep;
        }

        public void PlaySound(AudioSource audioSource, float volumeStart)
        {
            if (audioSource.clip != null)
            {
                audioSource.volume = volumeStart;
                audioSource.Play();
            }
        }

        public void StopSound(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}
