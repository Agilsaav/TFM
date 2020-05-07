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
        bool InstatiateElement = false;

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
            if(other.tag == "MainCamera" && !InstatiateElement)
            {
                PlaySound();
                manager.InstantiateNextElement();
                InstatiateElement = true;
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

