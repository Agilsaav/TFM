using UnityEngine;


//Human sounds Spawner class it spawn the sound at a random position around the spawner.
namespace WavesBehavior
{
    public class HumanSoundSpawner : MonoBehaviour
    {
        [SerializeField] HumanSoundBehavior humanSoundBehavior;
        [SerializeField] float spawnRadius = 10.0f;
        [SerializeField] float maxTimeBetweenSpawn = 120.0f;
        [SerializeField] float minTimeBetweenSpawn = 40.0f;

        float timer = 0.0f;
        float spawnTime;


        private void Start()
        {
            timer = 0.0f;
            spawnTime = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
        }

        private void Update()
        {
            if (timer >= spawnTime) InstantiateSound();
            timer += Time.deltaTime;
        }

        public void InstantiateSound()
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            HumanSoundBehavior agentInst = Instantiate(humanSoundBehavior, pos, Quaternion.identity);
            agentInst.transform.parent = gameObject.transform;

            timer = 0.0f;
            spawnTime = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
        }
    }
}
