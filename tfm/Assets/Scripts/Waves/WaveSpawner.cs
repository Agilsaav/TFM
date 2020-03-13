using System.Collections;
using UnityEngine;

namespace WavesBehavior
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] GameObject waveObject;
        [SerializeField] int numberOfWaves = 5;
        [SerializeField] float secondsBetweenWaves = 0.5f;
        [SerializeField] bool activateCollisions = false;
        [SerializeField] float timeBetweenWavesCollision = 5.0f;
        [SerializeField] bool SpawnInTime = false;
        [SerializeField] float secondsBetweenSpawn = 3.0f;
        [SerializeField] float volumeStart = 0.5f;
        [SerializeField] float volumeStep = 0.005f;

        [SerializeField] AudioClip clip = null;

        AudioSource audioSource;
        WaveSoundManager waveSoundManager;
        bool audioPlaying = false;

        float timer = 0.0f;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            waveSoundManager = FindObjectOfType<WaveSoundManager>();
            waveSoundManager.AssingClip(audioSource, clip);
        }

        private void Start()
        {
            if (activateCollisions) timer = timeBetweenWavesCollision;
            if (SpawnInTime) timer = Random.Range(0.0f, secondsBetweenSpawn);
        }

        void Update()
        {
            if (SpawnInTime && (timer >= secondsBetweenSpawn))
            {
                timer = 0.0f;
                StartCoroutine(SpawnWaves());
            }

            if (clip != null && audioPlaying) waveSoundManager.ChangeVolume(audioSource, volumeStep);

            timer += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (activateCollisions && timer >= timeBetweenWavesCollision)
            {
                timer = 0.0f;
                StartCoroutine(SpawnWaves());
            }
        }

        public void SpawnMainWave()
        {
            if (!activateCollisions && !SpawnInTime)
                StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            waveSoundManager.PlaySound(audioSource, volumeStart);
            audioPlaying = true;
            for (int i = 0; i < numberOfWaves; i++)
            {
                GameObject wave = Instantiate(waveObject, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(secondsBetweenWaves);
            }
        }
    }
}
