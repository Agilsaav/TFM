using UnityEngine;

namespace WavesBehavior
{
    public class HumanSoundBehavior : MonoBehaviour
    {
        [Tooltip("Intensity reducing factor")]
        [SerializeField] float newIntensity = 1 / 5.0f;
        [SerializeField] float recoveryTime = 1.5f;

        [Header("Clips Settings")]
        [SerializeField] float startVolume = 0.8f;
        [SerializeField] float volumeStep = 0.0001f;
        [SerializeField] float audioLength = 5.0f;
        [SerializeField] AudioClip[] clips;

        [Header("Blur Effect Settings")]
        [Range(0.1f, 20f), Tooltip("Blur Noise Scale.")]
        [SerializeField] float noiseScale = 2.5f;
        [Range(0.1f, 20f), Tooltip(" Blur Noise Frequency.")]
        [SerializeField] float noiseFreq = 5.0f;
        [Range(0.1f, 30f), Tooltip("Blur Noise Speed.")]
        [SerializeField] float noiseSpeed = 10.0f;
        [Range(0.001f, 0.1f), Tooltip("Blur PixelOffset.")]
        [SerializeField] float pixelOffset = 0.01f;
        [Tooltip("Blur PixelOffset Step.")]
        [SerializeField] float pixelStep = 0.001f;

        AudioSource audioSource;
        HumanSoundBehavior humanSoundBehavior;
        WaveSoundManager waveSoundManager;
        EffectsModifier effectsModifier;
        WaveBehavior[] waves;
        float startIntensity, timer;
        bool recovering = false;


        private void Awake()
        {
            effectsModifier = Camera.main.GetComponent<EffectsModifier>();
            humanSoundBehavior = GetComponent<HumanSoundBehavior>();
            audioSource = GetComponent<AudioSource>();
            waveSoundManager = FindObjectOfType<WaveSoundManager>();
            waveSoundManager.AssingClip(audioSource, clips[Random.Range(0, clips.Length)]);
            startIntensity = 1.0f / newIntensity;
        }

        private void Start()
        {
            SpawnSound();
        }

        private void Update()
        {
            if (recovering)
            {
                waveSoundManager.ChangeVolume(audioSource, volumeStep);
                effectsModifier.ReduceBlur(pixelStep);
                timer += Time.deltaTime;
                if (timer >= recoveryTime)
                {
                    humanSoundBehavior.ChangeWavesIntensity(startIntensity);
                    waveSoundManager.StopSound(audioSource);
                    recovering = false;
                    DestroySound();
                }
            }
        }

        private void SearchWaves()
        {
            waves = FindObjectsOfType<WaveBehavior>();
        }

        private void ChangeWavesIntensity(float intensity)
        {
            foreach (WaveBehavior wave in waves)
            {
                if(wave != null)
                {
                    Renderer renderer = wave.GetComponent<Renderer>();
                    Color color = renderer.material.GetColor("_Emission");
                    renderer.material.SetColor("_Emission", color * intensity);
                }
            }
        }

        private void SpawnSound()
        {
            waveSoundManager.PlaySound(audioSource, startVolume);
            effectsModifier.ActivateBlurEffect(noiseScale, noiseFreq, noiseSpeed, pixelOffset);
            SearchWaves();
            ChangeWavesIntensity(newIntensity);
            timer = 0.0f;
            recovering = true;
        }

        private void DestroySound()
        {
            effectsModifier.DeactivateBlur();
            Destroy(gameObject, audioLength);
        }
    }
}

