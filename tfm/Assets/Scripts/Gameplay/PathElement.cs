using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PathElement : MonoBehaviour
    {
        [SerializeField] AudioClip clip;

        PathManager manager;
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            manager = FindObjectOfType<PathManager>();
            audioSource.clip = clip;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "MainCamera")
            {
                PlaySound();
                manager.InstantiateNextElement();
                Destroy(gameObject, audioSource.clip.length);
            }

        }

        private void PlaySound()
        {
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

    }
}

